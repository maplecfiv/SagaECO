using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Scripting;

namespace SagaMap.Manager
{
    public class ScriptManager : Singleton<ScriptManager>
    {
        private string path;

        public ScriptManager()
        {
            Events = new Dictionary<uint, Event>();
        }

        public Dictionary<uint, Event> Events { get; }

        public ActorPC VariableHolder { get; set; }

        public Dictionary<string, MultiRunTask> Timers { get; } = new Dictionary<string, MultiRunTask>();

        public void ReloadScript()
        {
            ClientManager.NoCheckDeadLock = true;
            try
            {
                Events.Clear();
                LoadScript(path);
            }
            catch
            {
            }

            ClientManager.NoCheckDeadLock = false;
        }

        public void LoadScript(string path)
        {
            Logger.getLogger().Information("Loading uncompiled scripts");
            var dic = new Dictionary<string, string> { { "CompilerVersion", "v4.0" } };
            var provider = new CSharpCodeProvider(dic);
            var eventcount = 0;
            this.path = path;
            try
            {
                var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
                Assembly newAssembly;
                int tmp;
                if (files.Length > 0)
                {
                    newAssembly = CompileScript(files, provider);
                    if (newAssembly != null)
                    {
                        tmp = LoadAssembly(newAssembly);
                        Logger.getLogger().Information(string.Format("Containing {0} Events", tmp));
                        eventcount += tmp;
                    }
                }

                Logger.getLogger().Information("Loading compiled scripts....");
                files = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
                foreach (var i in files)
                {
                    newAssembly = Assembly.LoadFile(Path.GetFullPath(i));
                    if (newAssembly != null)
                    {
                        tmp = LoadAssembly(newAssembly);
                        Logger.getLogger().Information(string.Format("Loading {1}, Containing {0} Events", tmp, i));
                        eventcount += tmp;
                    }
                }

                if (!Events.ContainsKey(12001501))
                    Events.Add(12001501, new DungeonNorth());
                if (!Events.ContainsKey(12001502))
                    Events.Add(12001502, new DungeonEast());
                if (!Events.ContainsKey(12001503))
                    Events.Add(12001503, new DungeonSouth());
                if (!Events.ContainsKey(12001504))
                    Events.Add(12001504, new DungeonWest());
                if (!Events.ContainsKey(12001505))
                    Events.Add(12001505, new DungeonExit());
                if (!Events.ContainsKey(0xF1000000))
                    Events.Add(0xF1000000, new WestFortGate());
                if (!Events.ContainsKey(0xF1000001))
                    Events.Add(0xF1000001, new WestFortField());
            }
            catch (Exception ex)
            {
                Logger.getLogger().Error(ex, ex.Message);
            }


            Logger.getLogger().Information(string.Format("Totally {0} Events Added", eventcount));
        }

        private Assembly CompileScript(string[] Source, CSharpCodeProvider Provider)
        {
            //ICodeCompiler compiler = Provider.;
            var parms = new CompilerParameters();
            CompilerResults results;

            // Configure parameters
            parms.CompilerOptions = "/target:library /optimize";
            parms.GenerateExecutable = false;
            parms.GenerateInMemory = true;
            parms.IncludeDebugInformation = true;
            parms.ReferencedAssemblies.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) +
                                           @"\Reference Assemblies\Microsoft\Framework\v3.5\System.Data.DataSetExtensions.dll");
            parms.ReferencedAssemblies.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) +
                                           @"\Reference Assemblies\Microsoft\Framework\v3.5\System.Core.dll");
            parms.ReferencedAssemblies.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) +
                                           @"\Reference Assemblies\Microsoft\Framework\v3.5\System.Xml.Linq.dll");
            parms.ReferencedAssemblies.Add("System.dll");
            parms.ReferencedAssemblies.Add("SagaLib.dll");
            parms.ReferencedAssemblies.Add("SagaDB.dll");
            parms.ReferencedAssemblies.Add("SagaMap.exe");
            foreach (var i in Configuration.Configuration.Instance.ScriptReference) parms.ReferencedAssemblies.Add(i);
            // Compile
            results = Provider.CompileAssemblyFromFile(parms, Source);
            if (results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                    if (!error.IsWarning)
                    {
                        Logger.getLogger().Error("Compile Error:" + error.ErrorText, null);
                        Logger.getLogger().Error("File:" + error.FileName + ":" + error.Line, null);
                    }

                return null;
            }

            //get a hold of the actual assembly that was generated
            return results.CompiledAssembly;
        }

        private int LoadAssembly(Assembly newAssembly)
        {
            var newScripts = newAssembly.GetModules();
            var count = 0;
            foreach (var newScript in newScripts)
            {
                var types = newScript.GetTypes();
                foreach (var npcType in types)
                {
                    try
                    {
                        if (npcType.IsAbstract) continue;
                        if (npcType.GetCustomAttributes(false).Length > 0) continue;
                        Event newEvent;
                        try
                        {
                            newEvent = (Event)Activator.CreateInstance(npcType);
                        }
                        catch (Exception exception)
                        {
                            Logger.getLogger().Error(exception, null);
                            continue;
                        }

                        if (!Events.ContainsKey(newEvent.EventID) && newEvent.EventID != 0)
                        {
                            Events.Add(newEvent.EventID, newEvent);
                        }
                        else
                        {
                            if (newEvent.EventID != 0)
                                Logger.getLogger().Warning(string.Format("EventID:{0} already exists, Class:{1} droped",
                                    newEvent.EventID, npcType.FullName));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.getLogger().Error(ex, ex.Message);
                    }

                    count++;
                }
            }

            return count;
        }

        public uint GetFreeIDSince(uint since, int max)
        {
            for (var i = since; i < since + max; i++)
                if (!Events.ContainsKey(i))
                    return i;
            return 0xFFFFFFFF;
        }
    }
}
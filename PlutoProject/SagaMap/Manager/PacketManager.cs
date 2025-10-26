using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SagaLib;

namespace SagaMap.Manager {
    public class PacketManager : Singleton<PacketManager> {
        private string path;
        public List<uint> PacketsID { get; } = new List<uint>();

        public void LoadPacketFiles(string path) {
            var theList = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace == "SagaMap.Packets.Server")
                .ToList();

            // Logger.getLogger().Information("Loading uncompiled PacketFiles");
            // var dic = new Dictionary<string, string> { { "CompilerVersion", "v3.5" } };
            // var provider = new CSharpCodeProvider(dic);
            // Directory.SetCurrentDirectory(Directory.GetParent(path).FullName);
            // path = Directory.GetCurrentDirectory();
            // var i = path.LastIndexOf("\\");
            // path = path.Substring(0, i);
            // path = path + "\\SagaMap\\Packets\\Server";

            var Packetcount = 0;
            this.path = path;
            try {
                foreach (var newAssembly in theList) {
                    Packetcount += LoadAssembly(newAssembly.Assembly);
                }
                // var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
                // Assembly newAssembly;
                // int tmp;
                // if (files.Length > 0)
                // {
                //     newAssembly = CompilePacket(files, provider);
                //     if (newAssembly != null)
                //     {
                //         tmp = LoadAssembly(newAssembly);
                //         Logger.getLogger().Information(string.Format("Containing {0} Events", tmp));
                //         Packetcount += tmp;
                //     }
                // }

                Logger.ShowInfo($"{Packetcount} Packet Loaded");
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
        }

        private Assembly CompilePacket(string[] Source, CodeDomProvider Provider) {
            var parms = new CompilerParameters();
            CompilerResults results;
            parms.CompilerOptions = "/target:library /optimize";
            parms.GenerateExecutable = false;
            parms.GenerateInMemory = true;
            parms.IncludeDebugInformation = true;
            parms.ReferencedAssemblies.Add("System.dll");
            parms.ReferencedAssemblies.Add("SagaLib.dll");
            parms.ReferencedAssemblies.Add("SagaDB.dll");
            parms.ReferencedAssemblies.Add("SagaMap.exe");
            foreach (var i in Configuration.Configuration.Instance.ScriptReference) parms.ReferencedAssemblies.Add(i);
            // Compile
            results = Provider.CompileAssemblyFromFile(parms, Source);
            if (results.Errors.HasErrors) {
                foreach (CompilerError error in results.Errors)
                    if (!error.IsWarning) {
                        Logger.ShowError("Compile Error:" + error.ErrorText, null);
                        Logger.ShowError("File:" + error.FileName + ":" + error.Line, null);
                    }

                return null;
            }

            return results.CompiledAssembly;
        }

        private int LoadAssembly(Assembly newAssembly) {
            var newPackets = newAssembly.GetModules();
            var count = 0;
            foreach (var newScript in newPackets) {
                var types = newScript.GetTypes();
                foreach (var npcType in types) {
                    try {
                        if (npcType.IsAbstract) {
                            continue;
                        }

                        if (npcType.GetCustomAttributes(false).Length > 0) {
                            continue;
                        }

                        Packet newPacket = (Packet)Activator.CreateInstance(npcType);
                        if (newPacket == null) {
                            SagaLib.Logger.ShowWarning($"Cannot create new packet for {npcType}");
                            continue;
                        }

                        if (!PacketsID.Contains(newPacket.ID) && newPacket.ID != 0) {
                            PacketsID.Add(newPacket.ID);
                            count++;
                        }
                    }
                    catch (Exception ex) {
                        Logger.GetLogger().Warning(ex, ex.Message);
                    }
                }
            }

            return count;
        }
    }
}
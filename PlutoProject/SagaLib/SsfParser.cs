using System;
using System.Collections.Generic;
using System.IO;

namespace SagaLib {
    public class SsfParser {
        private Dictionary<string, string> GetData(string line) {
            foreach (var chk in new String[] { "[", "]" }) {
                while (line.Contains(chk)) {
                    line = line.Replace(chk, "");
                }
            }

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var data in line.Split(" ")) {
                if (!data.Contains("=")) {
                    continue;
                }

                string[] frag = data.Split("=");
                dict[frag[0].Trim()] = frag[1].Trim();
                if ($"{frag[0]}={frag[1]}" != data.Trim()) {
                    throw new Exception($"unparsable data {data}");
                }
            }

            return dict;
        }

        private void ProcessScript(string[] lines, HashSet<string> variables, List<string> outputs) {
            if (variables == null) {
                outputs.Add($"event_progresses:");
                outputs.Add($"  entrypoint:");
                outputs.Add($"    actions:");
                variables = new HashSet<string>();
            }

            Dictionary<string, string>
                varMap = new Dictionary<string, string>();

            Dictionary<string, Dictionary<string, string>> qaMap = new Dictionary<string, Dictionary<string, string>>();
            for (int i = 0; i < lines.Length; i++) {
                string line = lines[i].Trim();

                if (line.StartsWith("#")) {
                    // outputs.Add($"{line}");
                }
                else if (line.StartsWith("movenpc ")) {
                    var data = GetData((line));
                    var id = data["id"];
                    string[] npcIds;
                    if (varMap.ContainsKey(id)) {
                        var target = varMap[id];
                        if (target.Contains(",")) {
                            npcIds = target.Split(",");
                        }
                        else {
                            npcIds = new string[] { id };
                        }
                    }
                    else {
                        npcIds = new string[] { id };
                    }

                    outputs.Add($"      - action: move_npc");
                    outputs.Add($"        data: ");
                    outputs.Add($"          ids: ");
                    foreach (var npcId in npcIds) {
                        outputs.Add($"            - id: {npcId}");
                    }

                    // outputs.Add($"      id: {data["id"]}");
                    outputs.Add($"          x: {data["x"]}");
                    outputs.Add($"          y: {data["y"]}");
                    outputs.Add($"          speed: {data["speed"]}");
                    outputs.Add($"          dir: {data["dir"]}");
                    outputs.Add($"          move_type: {data["movetype"]}");
                    outputs.Add($"          motion: {data["motion"]}");
                    outputs.Add($"          motion_speed: {data["motionspeed"]}");
                }
                else if (line.StartsWith("hasitem ") || line.StartsWith("Hasitem ")) {
                    var data = GetData((line));
                    outputs.Add($"      - action: routine");
                    outputs.Add($"        data:");
                    outputs.Add($"          type: item");
                    outputs.Add($"          item: {data["id"]}");
                    outputs.Add($"          count: {data["count"]}");
                    outputs.Add($"          true: {data["label"]}");
                    outputs.Add($"          false: {data["else"]}");
                }
                else if (line.StartsWith("hasevent ")) {
                    var data = GetData((line));
                    outputs.Add($"      - action: routine");
                    outputs.Add($"        data:");
                    outputs.Add($"          type: event");
                    outputs.Add($"          event: {data["var"]}");
                    outputs.Add($"          ind: {data["ind"]}");
                    outputs.Add($"          true: {data["label"]}");
                    outputs.Add($"          false: {data["else"]}");
                }
                else if (line.StartsWith("goto ")) {
                    var data = GetData((line));
                    outputs.Add($"      - action: go_section");
                    outputs.Add($"        data:");
                    outputs.Add($"          section: {data["label"]}");
                }
                else if (line.StartsWith("npcflags ")) {
                    var data = GetData((line));
                    varMap["npcflags"] = data["ids"];
                }
                else if (line.StartsWith("global ")) {
                    // String[] data = line.Split(" ");
                    // outputs.Add($"define var {data[1]}");
                    // variables.Add(data[1]);
                }
                else if (line.StartsWith("mul ")) {
                    Dictionary<string, string> data = GetData(line);
                    var ori = varMap[data["var"]];
                    varMap[data["var"]] = $"{data["value"]}*={ori}";
                }
                else if (line.StartsWith("npcgroup ")) {
                    // outputs.Add($"Process npcgroup {line}");
                    string[] data = line.Split(" ");
                    // outputs.Add($"define local var {data[1]}");
                    variables.Add(data[1]);
                    varMap[data[1]] = data[2];
                }
                else if (line.StartsWith("*")) {
                    outputs.Add($"  {line.Substring(1)}:");
                    outputs.Add($"    actions:");
                    List<string> subLines = new List<string>();
                    for (int j = i + 1; j < lines.Length; i++, j++) {
                        if (lines[j].StartsWith("return")) {
                            subLines.Add(lines[j]);
                            i++;
                            break;
                        }
                        // else if (lines[j].StartsWith("end")) {
                        //     subLines.Add(lines[j]);
                        //     i++;
                        //     break;
                        // }
                        else if (lines[j].StartsWith("*")) {
                            i--;
                            break;
                        }
                        else {
                            // SagaLib.Logger.ShowInfo(($" {line} add {lines[j]}"));
                            subLines.Add(lines[j]);
                        }
                    }

                    ProcessScript(subLines.ToArray(), variables, outputs);
                }
                else if (line.StartsWith("talk")) {
                    Dictionary<string, string> data = GetData(line);
                    // outputs.Add($"Process dialog {line}");
                    List<string> subLines = new List<string>();
                    for (int j = i + 1; j < lines.Length; i++, j++) {
                        if (lines[j].StartsWith("end")) {
                            // subLines.Add(lines[j]);
                            i++;
                            break;
                        }
                        else {
                            subLines.Add(lines[j].Trim());
                        }
                    }

                    string imageId = data.ContainsKey("id") ? data["id"] : null;
                    // outputs.Add($"'{data["title"]}'({imageId}) says '{String.Join(
                    //     "", subLines)}'");
                    //
                    //
                    outputs.Add($"      - action: dialog");
                    outputs.Add($"        data:");
                    outputs.Add($"          title: {data["title"]}");
                    outputs.Add($"          content: {String.Join(
                        "", subLines)}");
                    if (imageId != null) {
                        outputs.Add($"          character_image: {imageId}");
                    }
                }
                else if (line.StartsWith("buyshop")) {
                    Dictionary<string, string> data = GetData(line);
                    // outputs.Add($"Process purchase dialog {line}");
                    List<string> subLines = new List<string>();
                    for (int j = i + 1; j < lines.Length; i++, j++) {
                        if (lines[j].StartsWith("end")) {
                            // subLines.Add(lines[j]);
                            i++;
                            break;
                        }
                        else {
                            subLines.Add(lines[j].Trim());
                        }
                    }


                    outputs.Add($"      - action: purchase_dialog");
                    outputs.Add($"        data:");
                    outputs.Add($"          items: ");
                    foreach (var itemId in subLines) {
                        outputs.Add($"            - {itemId}");
                    }
                }
                else if (line.StartsWith("select ")) {
                    // outputs.Add($"Process selection");
                    Dictionary<string, string> data = GetData(line);
                    for (int j = i + 1, oIdx = 0; j < lines.Length; i++, j++, oIdx++) {
                        if (lines[j].StartsWith("end")) {
                            i++;
                            break;
                        }

                        qaMap["question"] = new Dictionary<string, string>();
                        qaMap["question"]["title"] = data["title"];
                        qaMap["question"]["result"] = data["result"];
                        qaMap[oIdx.ToString()] = new Dictionary<string, string>();
                        qaMap[oIdx.ToString()]["Q"] = lines[j];
                    }
                }
                else if (line.StartsWith("case")) {
                    // outputs.Add($"Process selection condition");
                    Dictionary<string, string> data = GetData(line);
                    for (int j = i + 1, oIdx = 0; j < lines.Length; i++, j++, oIdx++) {
                        if (lines[j].StartsWith("end")) {
                            i++;
                            break;
                        }

                        if (!qaMap.ContainsKey(oIdx.ToString())) {
                            qaMap[oIdx.ToString()] = new Dictionary<string, string>();
                        }

                        qaMap[oIdx.ToString()]["A"] =
                            lines[j].Substring(lines[j].IndexOf("goto label=") + "goto label=".Length);
                    }

                    if (qaMap.ContainsKey("question")) {
                        outputs.Add($"      - action: routine");
                        outputs.Add($"        data:");
                        outputs.Add($"          type: question");
                        outputs.Add($"          question: {qaMap["question"]["title"]}");
                    }
                    else {
                        outputs.Add($"      - action: routine");
                        outputs.Add($"        data:");
                        outputs.Add($"          type: variable");
                        outputs.Add($"          var: {data["var"]}");
                    }

                    outputs.Add($"          options:");
                    foreach (string key in qaMap.Keys) {
                        if (key == "question") {
                            continue;
                        }

                        if (qaMap.ContainsKey("question")) {
                            if (qaMap[key].ContainsKey("A")) {
                                outputs.Add($"            {qaMap[key]["Q"]}: {qaMap[key]["A"].Trim()}");
                            }
                            else {
                                outputs.Add($"            {qaMap[key]["Q"]}: RETURN");
                            }
                        }
                        else {
                            outputs.Add($"            {key}: {qaMap[key]["A"].Trim()}");
                        }
                    }

                    qaMap.Clear();
                }
                else if (line.StartsWith("warp ")) {
                    var data = GetData(line);
                    outputs.Add($"      - action: teleport");
                    outputs.Add($"        data:");
                    outputs.Add($"          map_id: {data["map"]}");
                    outputs.Add($"          x: {data["x"]}");
                    outputs.Add($"          y: {data["y"]}");
                    outputs.Add($"          dir: {data["dir"]}");
                }
                else if (line.StartsWith("motion ")) {
                    Dictionary<string, string> data = GetData(line);
                    string[] targets;
                    if (varMap.ContainsKey(data["id"])) {
                        targets = varMap[data["id"]].Split(",");
                    }
                    else {
                        targets = new string[] { data["id"] };
                    }

                    outputs.Add($"      - action: play_motion");
                    outputs.Add($"        data:");
                    outputs.Add($"          motion: {data["motion"]}");
                    outputs.Add($"          ids:");
                    foreach (var target in targets) {
                        outputs.Add($"         - {target}");
                    }

                    if (data.ContainsKey("loop")) {
                        outputs.Add($"       loop: {data["loop"] == "1"}");
                    }
                }
                else if (line.StartsWith("stopbgm")) {
                    outputs.Add($"      - action: stop_bgm");
                    outputs.Add($"        type: stop_bgm");
                }
                else if (line.StartsWith("playbgm")) {
                    var data = GetData(line);
                    outputs.Add($"      - action: change_bgm");
                    outputs.Add($"        data:");
                    outputs.Add($"          id: {data["id"]}");
                }
                else if (line.StartsWith("screen")) {
                    var data = GetData(line);
                    outputs.Add($"      - action: control_screen");
                    outputs.Add($"        data:");
                    outputs.Add($"          enable: {data["enable"]}");
                    if (data.ContainsKey("color")) {
                        outputs.Add($"          color: {data["color"]}");
                    }
                }
                else if (line.StartsWith("wait ")) {
                    var data = GetData(line);
                    outputs.Add($"      - action: wait");
                    outputs.Add($"        data:");
                    outputs.Add($"          time: {data["time"]}");
                }
                else if (line.StartsWith("return")) {
                    // outputs.Add($"RETURN");
                }
                else if (line.StartsWith("loseitem ")) {
                    var data = GetData(line);
                    outputs.Add($"      - action: take_item");
                    outputs.Add($"        data:");
                    outputs.Add($"          id: {data["id"]}");
                    if (data.ContainsKey("count")) {
                        outputs.Add($"          count: {data["count"]}");
                    }
                }
                else if (line.StartsWith("getitem ")) {
                    var data = GetData(line);
                    outputs.Add($"      - action: give_item");
                    outputs.Add($"        data:");
                    outputs.Add($"          id: {data["id"]}");
                    if (data.ContainsKey("count")) {
                        outputs.Add($"          count: {data["count"]}");
                    }
                }
                else if (line.StartsWith("playse ")) {
                    var data = GetData(line);
                    outputs.Add($"      - action: play_se");
                    outputs.Add($"        data:");
                    outputs.Add($"          id: {data["id"]}");
                }
                else if (line.StartsWith("effect ")) {
                    var data = GetData(line);
                    outputs.Add($"      - action: play_effect");
                    outputs.Add($"        data:");
                    outputs.Add($"          id: {data["id"]}");
                    outputs.Add($"          effect: {data["effect"]}");
                }
                else if (line.StartsWith("end")) {
                    outputs.Add($"END");
                }
                else if (line.StartsWith("inc")) {
                    continue;
                }
                else if (line.StartsWith("set ")) {
                    Dictionary<string, string> data = GetData(line);
                    if (!data.ContainsKey("var") || !data.ContainsKey("value")) {
                        throw new Exception($"unparsable {line}");
                    }

                    // outputs.Add($"set var {data["var"]} = {data["value"]}");
                    varMap[data["var"]] = data["value"];

                    outputs.Add($"      - action: set_var");
                    outputs.Add($"        data:");
                    outputs.Add($"          name: {data["var"]}");
                    outputs.Add($"          value: {data["value"]}");
                }
                else if (line.Trim().Length == 0) {
                    continue;
                }
                else {
                    outputs.Add($"[UNKNOWN]{line}");
                }
            }
        }

        private void ProcessSsfFile(string eventId, Dictionary<string, string> eventMap) {
            List<string> outputs = new List<string>();
            List<string> _lines = new List<string>();
            string[] lines = null;
            foreach (var line in File.ReadAllLines(eventMap[eventId])) {
                if (!line.StartsWith("include")) {
                    _lines.Add(line);
                    continue;
                }

                lines = File.ReadAllLines(eventMap[line.Split(" ")[1].Trim()]);
                break;
            }

            if (lines == null) {
                lines = _lines.ToArray();
            }

            ProcessScript(lines, null, outputs);
            var fileName = Path.GetFileNameWithoutExtension(eventMap[eventId]);
            var dest = $"{ConfigLoader.LoadDbPath()}/Events/{fileName}.yaml";
            if (File.Exists(dest)) {
                File.Delete(dest);
            }

            Console.WriteLine(dest);
            File.AppendAllLines(dest, outputs);
        }

        private void Process() {
            var ssfFiles = Directory.GetFiles("/home/maple/Downloads/Serissa/scripts/", "*.ssf",
                SearchOption.AllDirectories);
            Dictionary<string, string> eventMap = new Dictionary<string, string>();
            foreach (var ssfFile in ssfFiles) {
                eventMap[Path.GetFileNameWithoutExtension(ssfFile)] = ssfFile;
            }

            foreach (var eventId in eventMap.Keys) {
                ProcessSsfFile(eventId, eventMap);
            }
        }

        public static void Main() {
            SsfParser ssfParser = new SsfParser();
            ssfParser.Process();
        }
    }
}
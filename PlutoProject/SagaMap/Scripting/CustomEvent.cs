using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;

namespace SagaMap.Scripting;

public class CustomEvent(EventData eventData) : Event {
    public EventData EventData = eventData;

    public override void OnEvent(ActorPC pc) {
        try {
            SagaDB.SqlSugarHelper.Db.BeginTran();
            EventProgress currentEventProgress = null;
            EventProgress nextEventProgress = EventData.EventProgresses[SagaLib.EventData.KeyEventEntrypoint];
            while (nextEventProgress != null) {
                currentEventProgress = nextEventProgress;
                nextEventProgress = null;
                foreach (var action in currentEventProgress.Actions) {
                    SagaLib.Logger.ShowInfo($"Process action {action.Action}");
                    switch (action.Action) {
                        case EventProgressType.DIALOG: {
                            var content = action.Data["content"].ToString()!;
                            if (content.Contains("$CHARACTER_NAME")) {
                                content = content.Replace("$CHARACTER_NAME", pc.Name);
                            }

                            var title = action.Data["title"] == null ? "" : action.Data["title"].ToString()!;
                            if (action.Data.ContainsKey("character_image")) {
                                Say(pc, uint.Parse(action.Data["character_image"].ToString()!), 131,
                                    content, title);
                            }
                            else {
                                Say(pc, 131, content,
                                    title);
                            }
                        }

                            break;
                        case EventProgressType.TELEPORT:
                            byte x = (byte)Random(byte.Parse(action.Data["x"].ToString()!),
                                byte.Parse(action.Data["x"].ToString()!));
                            byte y = (byte)Random(byte.Parse(action.Data["y"].ToString()!),
                                byte.Parse(action.Data["y"].ToString()!));
                            Warp(pc, uint.Parse(action.Data["map_id"].ToString()!), x, y);
                            break;
                        case EventProgressType.CHANGE_BGM:
                            ChangeBGM(pc, uint.Parse(action.Data["id"].ToString()!), true, 100, 50);
                            break;
                        case EventProgressType.GIVE_ITEM:
                            GiveItem(pc, uint.Parse(action.Data["id"].ToString()!),
                                action.Data.ContainsKey("count")
                                    ? ushort.Parse(action.Data["count"].ToString()!)
                                    : (ushort)1);
                            break;
                        case EventProgressType.GO_SECTION: {
                            var next = action.Data["section"].ToString()!.Trim();

                            if (next == "RETURN") {
                                SagaLib.Logger.ShowInfo($"will not be redirected to  {next}");
                            }
                            else if (EventData.EventProgresses[next].Actions == null) {
                                SagaLib.Logger.ShowInfo(
                                    $"will not be redirected to  {next}, which have no actions");
                            }
                            else {
                                nextEventProgress = EventData.EventProgresses[next];
                            }
                        }

                            break;
                        case EventProgressType.PLAY_EFFECT:
                            ShowEffect(pc, uint.Parse(action.Data["id"].ToString()!),
                                uint.Parse(action.Data["effect"].ToString()!));
                            break;
                        case EventProgressType.PLAY_MOTION:
                            foreach (var id in (List<string>)action.Data["ids"]) {
                                if (action.Data.ContainsKey("loop")) {
                                    NPCMotion(pc, uint.Parse(id), ushort.Parse(action.Data["motion"].ToString()!),
                                        bool.Parse(action.Data["loop"].ToString()!));
                                }
                                else {
                                    NPCMotion(pc, uint.Parse(id), ushort.Parse(action.Data["motion"].ToString()!));
                                }
                            }

                            break;
                        case EventProgressType.PLAY_SE:
                            PlaySound(pc, uint.Parse(action.Data["id"].ToString()!), false, 100, 50);
                            break;
                        case EventProgressType.STOP_BGM:
                            ChangeBGM(pc, 0, true, 0, 50);
                            break;
                        case EventProgressType.TAKE_ITEM:
                            TakeItem(pc, uint.Parse(action.Data["id"].ToString()!),
                                action.Data.ContainsKey("count")
                                    ? ushort.Parse(action.Data["count"].ToString()!)
                                    : (ushort)1);
                            break;
                        case EventProgressType.WAIT:
                            Wait(pc, uint.Parse(action.Data["time"].ToString()!));
                            break;
                        case EventProgressType.PURCHASE_DIALOG:
                            List<uint> itemIds = new List<uint>();
                            foreach (var itemId in (List<string>)action.Data["items"]) {
                                itemIds.Add(uint.Parse(itemId));
                            }

                            OpenShopByList(pc, itemIds.ToArray());
                            break;
                        case EventProgressType.MOVE_NPC:
                            foreach (var id in (List<string>)action.Data["ids"]) {
                                //uint npcID, byte x, byte y, ushort speed, byte type, ushort motion,
                                // ushort motionSpeed
                                NPCMove(pc, uint.Parse(id),
                                    byte.Parse(action.Data["x"].ToString()!),
                                    byte.Parse(action.Data["y"].ToString()!),
                                    ushort.Parse(action.Data["speed"].ToString()!),
                                    byte.Parse(action.Data["move_type"].ToString()!),
                                    ushort.Parse(action.Data["motion"].ToString()!),
                                    ushort.Parse(action.Data["motion_speed"].ToString()!));
                            }

                            break;
                        case EventProgressType.ROUTINE:
                            switch (action.Data["type"].ToString()!) {
                                case "item": {
                                    var count = CountItem(pc, uint.Parse(action.Data["item"].ToString()!));
                                    var next = action.Data["false"].ToString()!;
                                    if (count >= int.Parse(action.Data["count"].ToString()!)) {
                                        next = action.Data["true"].ToString()!;
                                    }

                                    next = next.Trim();

                                    if (next == "RETURN") {
                                        SagaLib.Logger.ShowInfo($"will not be redirected to  {next}");
                                    }
                                    else if (EventData.EventProgresses[next].Actions == null) {
                                        SagaLib.Logger.ShowInfo(
                                            $"will not be redirected to  {next}, which have no actions");
                                    }
                                    else {
                                        nextEventProgress = EventData.EventProgresses[next];
                                    }
                                }
                                    break;
                                case "question": {
                                    var options = (Dictionary<object, object>)action.Data["options"];
                                    List<string> optionKeys = new List<string>();
                                    foreach (var key in options.Keys) {
                                        optionKeys.Add(key.ToString());
                                    }

                                    var question = action.Data["question"].ToString()!;
                                    var answer = Select(pc, question, "", optionKeys.ToArray());

                                    SagaLib.Logger.ShowInfo(
                                        $"choose {answer}th in {String.Join(", ", optionKeys)} for {question}");

                                    var option =
                                        optionKeys[
                                            answer - 1];
                                    SagaLib.Logger.ShowInfo($"the answer is {option}");
                                    var next = options[option].ToString();
                                    next = next.Trim();
                                    SagaLib.Logger.ShowInfo($"going to  {next}");

                                    if (next == "RETURN") {
                                        SagaLib.Logger.ShowInfo($"will not be redirected to  {next}");
                                    }
                                    else if (EventData.EventProgresses[next].Actions == null) {
                                        SagaLib.Logger.ShowInfo(
                                            $"will not be redirected to  {next}, which have no actions");
                                    }
                                    else {
                                        nextEventProgress = EventData.EventProgresses[next];
                                    }
                                }

                                    break;
                                case "event": {
                                    var variableKey = $"EVENT_{action.Data["event"].ToString()!.ToUpper()}";
                                    var next = action.Data["false"].ToString()!;
                                    if (pc.CStr.ContainsKey(variableKey)) {
                                        if (pc.CStr[variableKey] == action.Data["ind"].ToString()!) {
                                            next = action.Data["true"].ToString()!;
                                        }
                                    }

                                    next = next.Trim();

                                    if (next == "RETURN") {
                                        SagaLib.Logger.ShowInfo($"will not be redirected to  {next}");
                                    }
                                    else if (EventData.EventProgresses[next].Actions == null) {
                                        SagaLib.Logger.ShowInfo(
                                            $"will not be redirected to  {next}, which have no actions");
                                    }
                                    else {
                                        nextEventProgress = EventData.EventProgresses[next];
                                    }
                                }
                                    break;
                                case "variable": {
                                    var variableKey = $"EVENT_{action.Data["var"].ToString()!.ToUpper()}";
                                    if (pc.CStr.ContainsKey(variableKey)) {
                                        var options = (Dictionary<object, object>)action.Data["options"];
                                        foreach (var key in options.Keys) {
                                            if (options[key].ToString() != pc.CStr[variableKey]) {
                                                continue;
                                            }

                                            var option = options[key].ToString();
                                            SagaLib.Logger.ShowInfo($"the answer is {option}");
                                            var next = options[option].ToString();
                                            next = next.Trim();
                                            SagaLib.Logger.ShowInfo($"going to  {next}");

                                            if (next == "RETURN") {
                                                SagaLib.Logger.ShowInfo($"will not be redirected to  {next}");
                                            }
                                            else if (EventData.EventProgresses[next].Actions == null) {
                                                SagaLib.Logger.ShowInfo(
                                                    $"will not be redirected to  {next}, which have no actions");
                                            }
                                            else {
                                                nextEventProgress = EventData.EventProgresses[next];
                                            }
                                        }
                                    }
                                }

                                    break;
                            }

                            break;
                        case EventProgressType.SET_VAR:
                            pc.CStr[$"EVENT_{action.Data["name"].ToString()!.ToUpper()}"] =
                                action.Data["value"].ToString()!;
                            break;
                        case EventProgressType.CONTROL_SCREEN:
                            break;
                        default:
                            SagaLib.Logger.ShowInfo($"Unknown action {action.Action}");
                            break;
                    }
                }
            }


            SagaDB.SqlSugarHelper.Db.CommitTran();
        }
        catch (Exception e) {
            SagaDB.SqlSugarHelper.Db.RollbackTran();
            SagaLib.Logger.ShowError(e);
        }
    }
}
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using SagaLib;
using SagaMap.Skill.SkillDefinations;
using SagaMap.Skill.SkillDefinations.Alchemist;
using SagaMap.Skill.SkillDefinations.Archer;
using SagaMap.Skill.SkillDefinations.Assassin;
using SagaMap.Skill.SkillDefinations.Astralist;
using SagaMap.Skill.SkillDefinations.Bard;
using SagaMap.Skill.SkillDefinations.Blacksmith;
using SagaMap.Skill.SkillDefinations.BladeMaster;
using SagaMap.Skill.SkillDefinations.BountyHunter;
using SagaMap.Skill.SkillDefinations.Breeder;
using SagaMap.Skill.SkillDefinations.Cabalist;
using SagaMap.Skill.SkillDefinations.Cardinal;
using SagaMap.Skill.SkillDefinations.Command;
using SagaMap.Skill.SkillDefinations.DarkStalker;
using SagaMap.Skill.SkillDefinations.Druid;
using SagaMap.Skill.SkillDefinations.Elementaler;
using SagaMap.Skill.SkillDefinations.Enchanter;
using SagaMap.Skill.SkillDefinations.Eraser;
using SagaMap.Skill.SkillDefinations.Event;
using SagaMap.Skill.SkillDefinations.Explorer;
using SagaMap.Skill.SkillDefinations.Farmasist;
using SagaMap.Skill.SkillDefinations.Fencer;
using SagaMap.Skill.SkillDefinations.FGarden;
using SagaMap.Skill.SkillDefinations.FL2_1;
using SagaMap.Skill.SkillDefinations.FL2_2;
using SagaMap.Skill.SkillDefinations.ForceMaster;
using SagaMap.Skill.SkillDefinations.FR2_1;
using SagaMap.Skill.SkillDefinations.FR2_2;
using SagaMap.Skill.SkillDefinations.Gambler;
using SagaMap.Skill.SkillDefinations.Gardener;
using SagaMap.Skill.SkillDefinations.Gladiator;
using SagaMap.Skill.SkillDefinations.Global;
using SagaMap.Skill.SkillDefinations.Guardian;
using SagaMap.Skill.SkillDefinations.Gunner;
using SagaMap.Skill.SkillDefinations.Harvest;
using SagaMap.Skill.SkillDefinations.Hawkeye;
using SagaMap.Skill.SkillDefinations.Item;
using SagaMap.Skill.SkillDefinations.Knight;
using SagaMap.Skill.SkillDefinations.Machinery;
using SagaMap.Skill.SkillDefinations.Maestro;
using SagaMap.Skill.SkillDefinations.Marionest;
using SagaMap.Skill.SkillDefinations.Marionette;
using SagaMap.Skill.SkillDefinations.Merchant;
using SagaMap.Skill.SkillDefinations.Monster;
using SagaMap.Skill.SkillDefinations.Necromancer;
using SagaMap.Skill.SkillDefinations.NewBoss;
using SagaMap.Skill.SkillDefinations.Ranger;
using SagaMap.Skill.SkillDefinations.Repair;
using SagaMap.Skill.SkillDefinations.Royaldealer;
using SagaMap.Skill.SkillDefinations.Sage;
using SagaMap.Skill.SkillDefinations.Scout;
using SagaMap.Skill.SkillDefinations.Shaman;
using SagaMap.Skill.SkillDefinations.Sorcerer;
using SagaMap.Skill.SkillDefinations.SoulTaker;
using SagaMap.Skill.SkillDefinations.Striker;
using SagaMap.Skill.SkillDefinations.Stryder;
using SagaMap.Skill.SkillDefinations.SunFlowerAdditions;
using SagaMap.Skill.SkillDefinations.Swordman;
using SagaMap.Skill.SkillDefinations.Tatarabe;
using SagaMap.Skill.SkillDefinations.Trader;
using SagaMap.Skill.SkillDefinations.Traveler;
using SagaMap.Skill.SkillDefinations.TreasureHunter;
using SagaMap.Skill.SkillDefinations.Vates;
using SagaMap.Skill.SkillDefinations.Warlock;
using SagaMap.Skill.SkillDefinations.Weapon;
using SagaMap.Skill.SkillDefinations.Wizard;
using SagaMap.Skill.SkillDefinations.X;
using AreaHeal = SagaMap.Skill.SkillDefinations.Druid.AreaHeal;
using AReflection = SagaMap.Skill.SkillDefinations.Sage.AReflection;
using Blow = SagaMap.Skill.SkillDefinations.Monster.Blow;
using Brandish = SagaMap.Skill.SkillDefinations.Monster.Brandish;
using CAPACommunion = SagaMap.Skill.SkillDefinations.Royaldealer.CAPACommunion;
using ChainLightning = SagaMap.Skill.SkillDefinations.Elementaler.ChainLightning;
using ChgTrance = SagaMap.Skill.SkillDefinations.Event.ChgTrance;
using ConfuseBlow = SagaMap.Skill.SkillDefinations.Monster.ConfuseBlow;
using CriUp = SagaMap.Skill.SkillDefinations.Royaldealer.CriUp;
using CureAll = SagaMap.Skill.SkillDefinations.Cardinal.CureAll;
using DarkMist = SagaMap.Skill.SkillDefinations.DarkStalker.DarkMist;
using DarkStorm = SagaMap.Skill.SkillDefinations.Monster.DarkStorm;
using EarthArrow = SagaMap.Skill.SkillDefinations.Monster.EarthArrow;
using EarthQuake = SagaMap.Skill.SkillDefinations.Astralist.EarthQuake;
using FireArrow = SagaMap.Skill.SkillDefinations.Monster.FireArrow;
using Gravity = SagaMap.Skill.SkillDefinations.Event.Gravity;
using Healing = SagaMap.Skill.SkillDefinations.Vates.Healing;
using Iai = SagaMap.Skill.SkillDefinations.Monster.Iai;
using IceArrow = SagaMap.Skill.SkillDefinations.Monster.IceArrow;
using LightOne = SagaMap.Skill.SkillDefinations.Monster.LightOne;
using MAG_INT_DEX_UP = SagaMap.Skill.SkillDefinations.Event.MAG_INT_DEX_UP;
using MagPoison = SagaMap.Skill.SkillDefinations.Monster.MagPoison;
using MagSlow = SagaMap.Skill.SkillDefinations.Monster.MagSlow;
using Phalanx = SagaMap.Skill.SkillDefinations.Monster.Phalanx;
using RobotAtkUp = SagaMap.Skill.SkillDefinations.Maestro.RobotAtkUp;
using RobotDefUp = SagaMap.Skill.SkillDefinations.Maestro.RobotDefUp;
using Rush = SagaMap.Skill.SkillDefinations.Monster.Rush;
using ShadowBlast = SagaMap.Skill.SkillDefinations.C1skill.ShadowBlast;
using ShockWave = SagaMap.Skill.SkillDefinations.ForceMaster.ShockWave;
using STR_VIT_AGI_UP = SagaMap.Skill.SkillDefinations.Event.STR_VIT_AGI_UP;
using StunBlow = SagaMap.Skill.SkillDefinations.Monster.StunBlow;
using SumMob = SagaMap.Skill.SkillDefinations.Global.SumMob;
using SumMobCastSkill = SagaMap.Skill.SkillDefinations.Global.SumMobCastSkill;
using TrDrop2 = SagaMap.Skill.SkillDefinations.Monster.TrDrop2;
using TurnUndead = SagaMap.Skill.SkillDefinations.Vates.TurnUndead;
using WaterArrow = SagaMap.Skill.SkillDefinations.Monster.WaterArrow;
using WaterGroove = SagaMap.Skill.SkillDefinations.Monster.WaterGroove;
using WindArrow = SagaMap.Skill.SkillDefinations.Monster.WindArrow;

namespace SagaMap.Skill
{
    public partial class SkillHandler : Singleton<SkillHandler>
    {
        public Dictionary<uint, MobISkill> MobskillHandlers = new Dictionary<uint, MobISkill>();

        private string path;
        public Dictionary<uint, ISkill> skillHandlers = new Dictionary<uint, ISkill>();
        private uint skillID;

        public void LoadSkill(string path)
        {
            Logger.ShowInfo("開始加載技能...");
            var dic = new Dictionary<string, string> { { "CompilerVersion", "v3.5" } };
            var provider = new CSharpCodeProvider(dic);
            var skillcount = 0;
            this.path = path;
            try
            {
                var files = Directory.GetFiles(path, "*cs", SearchOption.AllDirectories);
                Assembly newAssembly;
                int tmp;
                if (files.Length > 0)
                {
                    newAssembly = CompileScript(files, provider);
                    if (newAssembly != null)
                    {
                        tmp = LoadAssembly(newAssembly);
                        Logger.ShowInfo(string.Format("Containing {0} Skills", tmp));
                        skillcount += tmp;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }

            Logger.ShowInfo(string.Format("外置技能加載數：{0}", skillcount));
        }

        private Assembly CompileScript(string[] Source, CodeDomProvider Provider)
        {
            //ICodeCompiler compiler = Provider.;
            var parms = new CompilerParameters();
            CompilerResults results;

            // Configure parameters
            parms.CompilerOptions = "/target:library /optimize";
            parms.GenerateExecutable = false;
            parms.GenerateInMemory = true;
            parms.IncludeDebugInformation = true;
            //parms.ReferencedAssemblies.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Reference Assemblies\Microsoft\Framework\v3.5\System.Data.DataSetExtensions.dll");
            //parms.ReferencedAssemblies.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Reference Assemblies\Microsoft\Framework\v3.5\System.Core.dll");
            //parms.ReferencedAssemblies.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Reference Assemblies\Microsoft\Framework\v3.5\System.Xml.Linq.dll");
            parms.ReferencedAssemblies.Add("System.dll");
            parms.ReferencedAssemblies.Add("SagaLib.dll");
            parms.ReferencedAssemblies.Add("SagaDB.dll");
            parms.ReferencedAssemblies.Add("SagaMap.exe");
            foreach (var i in Configuration.Instance.ScriptReference) parms.ReferencedAssemblies.Add(i);
            // Compile
            results = Provider.CompileAssemblyFromFile(parms, Source);
            if (results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                    if (!error.IsWarning)
                    {
                        Logger.ShowError("Compile Error:" + error.ErrorText, null);
                        Logger.ShowError("File:" + error.FileName + ":" + error.Line, null);
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
                        ISkill newEvent;

                        newEvent = (ISkill)Activator.CreateInstance(npcType);
                        var t = newEvent.ToString();
                        var id = t.Substring(t.LastIndexOf("S") + 1, t.Length - t.LastIndexOf("S") - 1);
                        skillID = uint.Parse(id);

                        if (!skillHandlers.ContainsKey(skillID) && skillID != 0)
                        {
                            skillHandlers.Add(skillID, newEvent);
                        }
                        else
                        {
                            if (skillID != 0)
                                Logger.ShowWarning(string.Format("EventID:{0} already exists, Class:{1} droped",
                                    skillID, npcType.FullName));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ShowError(ex);
                    }

                    count++;
                }
            }

            return count;
        }

        public void Init()
        {
            skillHandlers.Add(2178, new EmergencyAvoid());

            MobskillHandlers.Add(20017, new BlackHoleOfPP());

            skillHandlers.Add(9125, new DeathFiger());
            //skillHandlers.Add(2115, new SagaMap.Skill.SkillDefinations.Event.PressionKiller(true));

            #region 巨大咕咕鸡

            MobskillHandlers.Add(20000, new BlackHole());
            MobskillHandlers.Add(20001, new GuguPoison());

            #endregion

            #region 熊爹

            MobskillHandlers.Add(20005, new IceHole()); //废弃
            MobskillHandlers.Add(20006, new Rowofcloudpalm()); //废弃
            MobskillHandlers.Add(20007, new Fengshenlegs()); //废弃
            MobskillHandlers.Add(20009, new Attack());

            #endregion

            #region 领主骑士

            MobskillHandlers.Add(20010, new KnightAttack());
            MobskillHandlers.Add(20011, new IceHeart());
            MobskillHandlers.Add(20012, new Iceroad());
            MobskillHandlers.Add(20013, new IceDef());

            #endregion

            #region 天骸鸢

            MobskillHandlers.Add(20015, new FireInfernal());

            #endregion

            skillHandlers.Add(20002, new EnergyOneForWeapon());
            skillHandlers.Add(20004, new IaiForWeapon());
            skillHandlers.Add(402, new MaxHealMpForWeapon());
            skillHandlers.Add(20014, new Snipe());

            #region c-1 new skill

            skillHandlers.Add(8900, new ShadowBlast());

            #endregion

            #region Royaldealer

            skillHandlers.Add(989, new DealerSkill()); //18.05.13 lv 3  
            skillHandlers.Add(3361, new CAPACommunion()); //12月2日实装,lv6
            skillHandlers.Add(3371, new RoyalDealer()); //18.05.13 lv 10
            skillHandlers.Add(2491, new DamageUp()); //12月3日实装,lv13
            //noLv20
            skillHandlers.Add(2501, new CriUp()); //12月3日实装,lv23
            skillHandlers.Add(2502, new PerfectRiotStamp()); //18.05.13 lv 25
            //noLv25
            skillHandlers.Add(3404, new Rhetoric()); //12月3日实装,lv30(强运系统未装)
            skillHandlers.Add(2517, new StraightFlush()); //18.05.13 lv 35
            skillHandlers.Add(2518, new StraightFlushSEQ()); //18.05.13 lv 35
            skillHandlers.Add(1114, new LuckyGoddess()); //18.08.04 lv 40
            skillHandlers.Add(2558, new FalseMoney()); //18.05.13 lv 47
            skillHandlers.Add(2559, new TimeIsMoney()); //16.06.08 lv50
            //no 35 40 45

            #endregion

            #region Joker

            skillHandlers.Add(2519, new JokerStyle()); //9月9日实装
            skillHandlers.Add(2523, new IkspiariArmusing()); //9月9日实装
            skillHandlers.Add(2494, new JokerDelay()); //9月9日实装
            skillHandlers.Add(3390, new JokerArt()); //9月9日实装
            skillHandlers.Add(2483, new JokerStrike()); //9月9日实装
            skillHandlers.Add(990, new FullWeaponMaster()); //9月9日实装
            skillHandlers.Add(991, new FullerMaster()); //9月9日实装
            skillHandlers.Add(3410, new DivineProtection()); //9月9日实装
            skillHandlers.Add(3437, new StyleChange()); //9月9日实装
            skillHandlers.Add(2560, new JokerNone()); //9月9日实装,未完成
            skillHandlers.Add(2562, new JokerTwoHead()); //9月9日实装,未完成
            skillHandlers.Add(2566, new Joker()); //9月9日实装,未完成

            #endregion

            #region Stryder

            skillHandlers.Add(2482, new Xusihaxambi()); //2018年1月8日实装，lv3
            skillHandlers.Add(3352, new SPCommunion()); //12月2日实装，lv6
            //lv10被动不知作用
            skillHandlers.Add(2490, new StrapFlurry()); //2018年1月8日实装，lv13
            skillHandlers.Add(3382, new BannedOutfit()); //12月2日实装，lv20（只实现显示BUFF效果）  
            //lv23被动不知作用
            skillHandlers.Add(3385, new SkillForbid()); //12月2日实装，lv25（只实现显示BUFF效果）
            skillHandlers.Add(3402, new PartyBivouac()); //12月2日实装，lv30
            skillHandlers.Add(3403, new FlurryThunderbolt()); //12月2日实装，lv35
            skillHandlers.Add(992, new TreasureMaster()); //2018/4/5实装,job45
            skillHandlers.Add(2551, new PillageAct()); //2018/4/5实装,job47
            skillHandlers.Add(2552, new ArtFullTrap()); //2018/5/14,job50
            //缺少35、40

            #endregion

            #region Maestro

            skillHandlers.Add(2480, new WeaponStrengthen()); //12月1日实装，lv3（未完成，需要封包）
            skillHandlers.Add(3353, new ATKCommunion()); //12月1日实装，lv6
            skillHandlers.Add(987, new GreatMaster()); //12月1日实装,lv10（加成不明确）
            skillHandlers.Add(2487, new PotentialWeapon()); //12月1日实装，lv13（未完成，需要封包）
            skillHandlers.Add(2489, new RobotAtkUp()); //12月1日实装，lv20
            skillHandlers.Add(2500, new RobotDefUp()); //12月1日实装，lv23
            skillHandlers.Add(2506, new RobotCSPDUp()); //12月1日实装，lv25
            skillHandlers.Add(3401, new WeaponAtkUp()); //12月1日实装，lv30
            //缺少35、40
            skillHandlers.Add(2524, new RobotLaser()); //12月2日实装，lv45
            skillHandlers.Add(2549, new LimitExceed()); //7月1日实装，lv47
            skillHandlers.Add(2550, new WasteThrowing()); //2018.1.17实装,lv50

            #endregion

            #region Guardian

            skillHandlers.Add(983, new SpearMaster()); //11月24日实装，LV3习得
            skillHandlers.Add(3355, new def_addCommunion());
            skillHandlers.Add(3363, new Guardian()); //11月24日实装，LV10习得
            skillHandlers.Add(1102, new ReflectionShield()); //11月24日实装，LV20习得
            skillHandlers.Add(3386, new SoulProtect()); //8月1日实装，lv25习得
            skillHandlers.Add(2512, new ShieldImpact()); //11月24日实装，lv30习得
            skillHandlers.Add(2513, new SpiralSpear()); //11月24日实装，lv35习得
            skillHandlers.Add(2533, new StrongBody()); //11月24日实装，lv40习得
            skillHandlers.Add(2532, new Blocking()); //11月24日实装，lv45习得
            skillHandlers.Add(1101, new HatredUp()); //11月25日实装，lv13习得
            skillHandlers.Add(2535, new FortressCircle()); //2018年1月10日实装,Lv47习得//未完善
            skillHandlers.Add(2536, new FortressCircleSEQ()); //后续技能,同上
            skillHandlers.Add(2537, new LightOfTheDarkness()); //16.02.02, lv50

            #endregion

            #region Eraser

            skillHandlers.Add(984, new EraserMaster()); //11月24日实装，lv3习得
            skillHandlers.Add(3358, new AVOIDCommunion());
            skillHandlers.Add(3364, new Purger()); //11月24日实装，lv10习得
            skillHandlers.Add(2486, new Efuikasu()); //11月24日实装，Lv20习得
            skillHandlers.Add(3387, new Syaringan()); //11月24日实装，lv25习得
            skillHandlers.Add(2516, new EvilSpirit()); //11月24日实装，lv30习得
            skillHandlers.Add(2508, new Demacia()); //11月24日实装，lv35习得
            skillHandlers.Add(2529, new ShadowSeam()); //11月24日实装，lv40习得//未完成
            skillHandlers.Add(994, new PoisonMaster()); //2018年1月11日实装，lv45习得
            skillHandlers.Add(2541, new VenomBlast()); //2018年实装，lv47习得
            skillHandlers.Add(2542, new VenomBlastSeq()); //2018年实装，lv47习得
            skillHandlers.Add(2543, new Instant()); //16.05.11实装,lv50

            #endregion

            #region Hawkeye

            skillHandlers.Add(3357, new HITCommunion());
            skillHandlers.Add(985, new HawkeyeMaster()); //11月24日实装，lv3习得
            skillHandlers.Add(3365, new EagleEye()); //11月24日实装，lv10习得
            skillHandlers.Add(1103, new Nooheito()); //11月25日实装，lv13习得
            skillHandlers.Add(2485, new SmokeBall()); //11月25日实装，lv20习得
            skillHandlers.Add(1107, new MissRevenge()); //11月25日实装，lv23习得
            skillHandlers.Add(2504, new WithinWeeks()); //11月25日实装，lv25习得
            skillHandlers.Add(2514, new TimeBomb()); //11月25日实装，lv30习得
            skillHandlers.Add(2515, new TimeBombSEQ()); //6月26日实装追加部分，lv30习得
            skillHandlers.Add(2507, new PointRain()); //11月25日实装，lv35习得
            skillHandlers.Add(2531, new LoboCall()); //11月25日实装，lv40习得
            skillHandlers.Add(2530, new SejiwuiPoint()); //11月25日实装，lv45习得
            skillHandlers.Add(2538, new ImpactShot()); //6月25日实装，lv47习得
            skillHandlers.Add(2539, new MirageShot()); //6月26日实装，lv50习得
            skillHandlers.Add(2540, new MirageShotSEQ()); //6月26日实装，lv50习得

            #endregion

            #region ForceMaster

            skillHandlers.Add(986, new PlusElement()); //11月25日实装
            skillHandlers.Add(3359, new CSPDCommunion());
            skillHandlers.Add(3366, new ForceMaster());
            skillHandlers.Add(3375, new DecreaseWeapon()); //11月25日实装
            skillHandlers.Add(1105, new ForceShield()); //11月25日实装
            skillHandlers.Add(3383, new DecreaseShield()); //11月26日实装
            skillHandlers.Add(3388, new BarrierShield()); //11月26日实装
            skillHandlers.Add(3395, new ForceWave()); //11月26日实装,lv30（未完成实装）
            skillHandlers.Add(3394, new ThunderSpray()); //11月26日实装，lv35
            skillHandlers.Add(3419, new AdobannaSubiritei()); //11月26日实装，lv40
            skillHandlers.Add(3418, new ShockWave()); //11月26日实装,lv45
            skillHandlers.Add(3430, new DispelField()); //16.02.08实装, lv47
            skillHandlers.Add(3428, new DeathTractionGlare()); //2016-01-30实装,lv50
            skillHandlers.Add(3429, new DeathTractionGlareSEQ());

            #endregion

            #region Astralist

            skillHandlers.Add(3372, new DelayOut()); //11月26日实装,lv3
            skillHandlers.Add(3351, new MPCommunion());
            skillHandlers.Add(3367, new Astralist()); //11月26日实装,lv10
            skillHandlers.Add(3377, new TranceBody()); //11月26日实装,lv13
            skillHandlers.Add(3378, new Relement()); //11月26日实装,lv20
            skillHandlers.Add(3384, new Amplement()); //11月26日实装,lv23
            skillHandlers.Add(3389, new YugenKeiyaku()); //11月26日实装,lv25
            skillHandlers.Add(3409, new ElementGun()); //11月29日实装,lv30
            skillHandlers.Add(3398, new EarthQuake()); //11月29日实装,lv35
            skillHandlers.Add(3417, new Contract()); //11月29日实装,lv40
            skillHandlers.Add(3433, new ElementMemory()); //03月29日实装,lv47
            skillHandlers.Add(3416, new WindExplosion()); //11月29日实装 lv45
            skillHandlers.Add(3432, new ElementStar()); //确定是JOB50技能，6.11修正

            #endregion

            #region Cardinal

            skillHandlers.Add(3373, new Frustrate()); //11月30日实装,lv3
            skillHandlers.Add(3356, new MDEFCommunion()); // lv6
            skillHandlers.Add(3379, new CureTheUndead()); //16.02.02实装,lv13
            skillHandlers.Add(3368, new Cardinal()); //11月30日实装 lv10
            skillHandlers.Add(3380, new CureAll()); //11月30日实装lv20
            skillHandlers.Add(1109, new AutoHeal()); //11月30日实装,lv25
            skillHandlers.Add(3399, new AngelRing()); //11月30日实装,lv30
            skillHandlers.Add(3393, new MysticShine()); //11月30日实装,lv35
            skillHandlers.Add(3415, new Recovery()); //11月30日实装,lv40
            skillHandlers.Add(3414, new DevineBreaker()); //11月30日实装,lv45
            skillHandlers.Add(3436, new Salvation()); // 16.01.08实装,lv47
            skillHandlers.Add(3434, new Gospel()); // 16.01.08实装,lv50

            #endregion

            #region SoulTaker

            skillHandlers.Add(3374, new MegaDarkBlaze());
            skillHandlers.Add(3354, new MATKCommunion());
            skillHandlers.Add(3369, new SoulTaker());
            skillHandlers.Add(2544, new SoulHunting()); //2018.1.17实装,lv47
            skillHandlers.Add(2545, new SoulHuntingSEQ()); //2018.1.17实装,lv47后续技能
            skillHandlers.Add(3376, new Transition()); //11月30日实装，lv20
            skillHandlers.Add(1110, new SoulTakerMaster()); //11月30日实装，lv23
            skillHandlers.Add(3397, new DarkChains()); //11月30日实装，lv30
            skillHandlers.Add(3392, new Chasm()); //11月30日实装，lv35
            skillHandlers.Add(3420, new DeathSickle()); //11月30实装,lv40
            skillHandlers.Add(2526, new fuenriru()); //11月30实装,lv45
            skillHandlers.Add(3431, new Dammnation()); //16.01.08实装,lv50

            #endregion

            skillHandlers.Add(1606, new Ryuugankakusen());
            skillHandlers.Add(1607, new DragonEyesOfGod());

            #region Harvest

            skillHandlers.Add(2481, new EquipCompose()); //12月2日实装，lv3（未完成，需要封包）
            skillHandlers.Add(3360, new SkillDefinations.Harvest.CAPACommunion()); //12月2日实装，lv6
            skillHandlers.Add(3370, new Twine()); //12月2日,lv10（未实装，可能需要新的debuff）
            skillHandlers.Add(2488, new PotentialArmor()); //12月2日，lv13（未完成，需要封包）
            skillHandlers.Add(3381, new TwineSleep()); //12月2日,lv20（未实装，可能需要新的debuff）
            skillHandlers.Add(2505, new EquipComposeCancel()); //12月2日实装，lv23（未完成，需要封包）
            skillHandlers.Add(2497, new Bounce()); //12月2日完成实装,lv25
            skillHandlers.Add(2510, new Winder()); //12月2日,lv30（未完成，需要封包）
            skillHandlers.Add(3400, new CreateNeko()); //12月2日,lv35（未完成，需要召唤物技能）
            skillHandlers.Add(3413, new TwistedPlant()); //未完成,Lv40
            skillHandlers.Add(993, new HarvestMaster()); //未完全完成,Lv45
            skillHandlers.Add(2547, new MistletoeShooting()); //2018.1.18实装,Lv47
            skillHandlers.Add(2548, new MistletoeShootingSEQ()); //2018.1.18实装,Lv47
            skillHandlers.Add(2546, new PlantField()); //2018.1.18实装,Lv50
            //缺少40、45

            #endregion


            skillHandlers.Add(101, new MaxMPUp());
            skillHandlers.Add(103, new HPRecoverUP());
            skillHandlers.Add(104, new MPRecoverUP());
            skillHandlers.Add(105, new SPRecoverUP());
            skillHandlers.Add(108, new AxeMastery());
            skillHandlers.Add(111, new MaceMastery());
            skillHandlers.Add(121, new TwoMaceMastery());
            skillHandlers.Add(128, new TwoHandGunMastery());
            skillHandlers.Add(200, new SwordHitUP());
            skillHandlers.Add(202, new SpearHitUP());
            skillHandlers.Add(204, new AxeHitUP());
            skillHandlers.Add(206, new ShortSwordHitUP());
            skillHandlers.Add(208, new MaceHitUP());
            skillHandlers.Add(113, new BowMastery());
            skillHandlers.Add(903, new Identify());
            skillHandlers.Add(907, new ThrowHitUP());
            skillHandlers.Add(2021, new Synthese());
            skillHandlers.Add(4026, new A_T_PJoint());
            skillHandlers.Add(978, new AtkUpByPt());
            skillHandlers.Add(959, new RiskAversion());
            skillHandlers.Add(712, new Camp());

            skillHandlers.Add(1602, new EventMoveSkill());

            skillHandlers.Add(10502, new Fish());
            skillHandlers.Add(904, new FoodFighter());
            skillHandlers.Add(2070, new SpeedUpSkill());
            skillHandlers.Add(2078, new MetalRepair());

            #region System

            skillHandlers.Add(3250, new FGRope());

            #endregion

            #region Mob

            skillHandlers.Add(7843, new ElementShield(Elements.Fire, true));
            skillHandlers.Add(7169, new StarLove());
            MobskillHandlers.Add(7866, new SomaMirage());
            skillHandlers.Add(6444, new Curse());
            skillHandlers.Add(7500, new PoisonPerfume());
            skillHandlers.Add(7501, new RockStone());
            skillHandlers.Add(7502, new ParaizBan());
            skillHandlers.Add(7503, new SleepStrike());
            skillHandlers.Add(7504, new SilentGreen());
            skillHandlers.Add(7505, new SlowLogic());
            skillHandlers.Add(7506, new ConfuseStack());
            skillHandlers.Add(7507, new IceFade());
            skillHandlers.Add(7508, new StunAttack());
            skillHandlers.Add(7509, new EnergyAttack());
            skillHandlers.Add(7510, new FireAttack());
            skillHandlers.Add(7511, new WaterAttack());
            skillHandlers.Add(7512, new WindAttack());
            skillHandlers.Add(7513, new EarthAttack());
            skillHandlers.Add(7514, new LightBallad());
            skillHandlers.Add(7515, new DarkBallad());
            skillHandlers.Add(7516, new Blow());
            skillHandlers.Add(7517, new ConfuseBlow());
            skillHandlers.Add(7518, new StunBlow());
            skillHandlers.Add(7519, new MobHealing());
            skillHandlers.Add(7520, new MobHealing1());
            skillHandlers.Add(7521, new MobAshibarai());
            skillHandlers.Add(7522, new Brandish());
            skillHandlers.Add(7523, new Rush());
            skillHandlers.Add(7524, new Iai());
            skillHandlers.Add(7525, new KabutoWari());
            skillHandlers.Add(7526, new MobBokeboke());
            skillHandlers.Add(7527, new SkillDefinations.Monster.ShockWave());
            skillHandlers.Add(7528, new MobStormSword());
            skillHandlers.Add(7529, new Phalanx());
            skillHandlers.Add(7530, new WarCry());
            skillHandlers.Add(7531, new ExciaMation());
            skillHandlers.Add(7532, new IceArrow());
            skillHandlers.Add(7533, new DarkOne());
            skillHandlers.Add(7534, new WaterStorm());
            skillHandlers.Add(7535, new DarkStorm());
            skillHandlers.Add(7536, new WaterGroove());
            skillHandlers.Add(7537, new MobParalyzeblow());
            skillHandlers.Add(7538, new MobFireart());
            skillHandlers.Add(7539, new MobWaterart());
            skillHandlers.Add(7540, new MobEarthart());
            skillHandlers.Add(7541, new MobWindart());
            skillHandlers.Add(7542, new MobLightart());
            skillHandlers.Add(7543, new MobDarkart());
            skillHandlers.Add(7544, new MobTrSilenceAtk());
            skillHandlers.Add(7545, new MobTrPoisonAtk());
            skillHandlers.Add(7546, new MobTrPoisonCircle());
            skillHandlers.Add(7547, new MobTrStuinCircle());
            skillHandlers.Add(7548, new MobTrSleepCircle());
            skillHandlers.Add(7549, new MobTrSilenceCircle());
            skillHandlers.Add(7550, new MagPoison());
            skillHandlers.Add(7551, new MagSleep());
            skillHandlers.Add(7552, new MagSlow());
            skillHandlers.Add(7553, new StoneCircle());
            skillHandlers.Add(7554, new HiPoisonCircie());
            skillHandlers.Add(7555, new IceCircle());
            skillHandlers.Add(7556, new HiPoison());
            skillHandlers.Add(7557, new DeadlyPoison());
            skillHandlers.Add(7558, new MobPerfectcritical());
            skillHandlers.Add(7559, new SumSlaveMob(10010100)); //古代咕咕雞
            skillHandlers.Add(7560, new SumSlaveMob(26000000)); //ブリキングRX１
            skillHandlers.Add(7561, new SumSlaveMob(10080100)); //テンタクル
            skillHandlers.Add(7562, new SumSlaveMob(10040100)); //ワスプ
            skillHandlers.Add(7563, new SumSlaveMob(10030400)); //ポーラーベア
            skillHandlers.Add(7564, new FireHighStorm());
            skillHandlers.Add(7565, new WindHighWave());
            skillHandlers.Add(7566, new WindHighStorm());
            skillHandlers.Add(7567, new FireOne());
            skillHandlers.Add(7568, new FireStorm());
            skillHandlers.Add(7569, new WindStorm());
            skillHandlers.Add(7570, new EarthStorm());
            skillHandlers.Add(7571, new LightOne());
            skillHandlers.Add(7572, new DarkHighOne());
            //skillHandlers.Add(7573, new SkillDefinations.Enchanter.PoisonMash(true));
            MobskillHandlers.Add(7573, new PoisonMash(true));
            skillHandlers.Add(7574, new MobAvoupSelf());
            skillHandlers.Add(7575, new EnergyShield(true));
            skillHandlers.Add(7576, new MagicShield(true));
            skillHandlers.Add(7577, new MobAtkupOne());
            skillHandlers.Add(7578, new MobCharge());
            skillHandlers.Add(7579, new SumSlaveMob(10030903)); //黑熊
            skillHandlers.Add(7580, new SumSlaveMob(26180002)); //皮格夫
            skillHandlers.Add(7581, new SumSlaveMob(26100003)); //木魚
            skillHandlers.Add(7582, new MobTrSleep());
            skillHandlers.Add(7583, new MobTrStun());
            skillHandlers.Add(7584, new MobTrSilence());
            skillHandlers.Add(7585, new MobConfPoisonCircle());
            skillHandlers.Add(7586, new ElementCircle(Elements.Fire, true));
            skillHandlers.Add(7587, new ElementCircle(Elements.Wind, true));
            skillHandlers.Add(7588, new ElementCircle(Elements.Water, true));
            skillHandlers.Add(7589, new ElementCircle(Elements.Earth, true));
            skillHandlers.Add(7590, new ElementCircle(Elements.Holy, true));
            skillHandlers.Add(7591, new ElementCircle(Elements.Dark, true));
            skillHandlers.Add(7592, new SumSlaveMob(26180000)); //得菩提
            skillHandlers.Add(7593, new SumSlaveMob(26100000)); //雷魚
            skillHandlers.Add(7594, new SumSlaveMob(10030900)); //黑熊
            skillHandlers.Add(7595, new SumSlaveMob(10310006)); //艾卡納J牌
            skillHandlers.Add(7596, new SumSlaveMob(10250003)); //得菩提
            skillHandlers.Add(7597, new SumSlaveMob(30490000, 1)); //巨大咕咕銅像
            skillHandlers.Add(7598, new SumSlaveMob(30500000, 1)); //破壞MkII銅像
            skillHandlers.Add(7599, new SumSlaveMob(30510000, 1)); //皇路普銅像
            skillHandlers.Add(7600, new SumSlaveMob(30520000, 1)); //螫針蜂銅像
            skillHandlers.Add(7601, new SumSlaveMob(30530000, 1)); //白熊銅像

            skillHandlers.Add(7605, new SumSlaveMob(30150005, 4)); //雑草
            skillHandlers.Add(7606, new MobMeteo());
            skillHandlers.Add(7607, new MobDoughnutFireWall());
            skillHandlers.Add(7608, new MobReflection());

            skillHandlers.Add(7609, new MobElementLoad(7664)); //燃燒的路
            skillHandlers.Add(7610, new MobElementLoad(7665)); //凍結的路
            skillHandlers.Add(7611, new MobElementLoad(7666)); //螺旋風！
            skillHandlers.Add(7612, new MobElementLoad(7667)); //私家路
            skillHandlers.Add(7613, new MobElementLoad(7668)); //死神
            skillHandlers.Add(7614, new MobElementRandcircle(7669, 5));
            skillHandlers.Add(7615, new MobElementRandcircle(7670, 5));
            skillHandlers.Add(7616, new MobElementRandcircle(7671, 5));
            skillHandlers.Add(7617, new MobElementRandcircle(7672, 5));
            skillHandlers.Add(7618, new MobElementRandcircle(7673, 5));
            skillHandlers.Add(7619, new MobElementRandcircle(7674, 3));
            skillHandlers.Add(7620, new MobElementRandcircle(7675, 3));
            skillHandlers.Add(7621, new MobElementRandcircle(7676, 3));
            skillHandlers.Add(7622, new MobElementRandcircle(7677, 3));
            skillHandlers.Add(7623, new MobElementRandcircle(7678, 3));

            skillHandlers.Add(7648, new MobVitdownOne());
            skillHandlers.Add(7649, new MobCircleAtkup());
            skillHandlers.Add(7650, new GravityFall(true));
            skillHandlers.Add(7651, new AReflection());
            skillHandlers.Add(7652, new DelayCancel());
            skillHandlers.Add(7653, new MobCharge3());
            skillHandlers.Add(7654, new SumMob(30150007));
            skillHandlers.Add(7655, new SumMob(30130003));
            skillHandlers.Add(7656, new SumMob(30130005));
            skillHandlers.Add(7657, new SumMob(30130007));
            skillHandlers.Add(7658, new SumMob(30070052));
            skillHandlers.Add(7659, new SumMob(30070054));
            skillHandlers.Add(7660, new SumMob(30070056));
            skillHandlers.Add(7661, new SumMob(30070058));
            skillHandlers.Add(7662, new SumMob(30070060));
            skillHandlers.Add(7663, new SumMob(30070062));
            skillHandlers.Add(7664, new MobElementLoadSeq(Elements.Fire)); //燃燒的路
            skillHandlers.Add(7665, new MobElementLoadSeq(Elements.Water)); //凍結的路
            skillHandlers.Add(7666, new MobElementLoadSeq(Elements.Wind)); //螺旋風！
            skillHandlers.Add(7667, new MobElementLoadSeq(Elements.Earth)); //私家路
            skillHandlers.Add(7668, new MobElementLoadSeq(Elements.Dark)); //死神

            skillHandlers.Add(7669, new MobElementRandcircleSeq(Elements.Fire));
            skillHandlers.Add(7670, new MobElementRandcircleSeq(Elements.Water));
            skillHandlers.Add(7671, new MobElementRandcircleSeq(Elements.Wind));
            skillHandlers.Add(7672, new MobElementRandcircleSeq(Elements.Earth));
            skillHandlers.Add(7673, new MobElementRandcircleSeq(Elements.Dark));
            skillHandlers.Add(7674, new MobElementRandcircleSeq(Elements.Fire));
            skillHandlers.Add(7675, new MobElementRandcircleSeq(Elements.Water));
            skillHandlers.Add(7676, new MobElementRandcircleSeq(Elements.Wind));
            skillHandlers.Add(7677, new MobElementRandcircleSeq(Elements.Earth));
            skillHandlers.Add(7678, new MobElementRandcircleSeq(Elements.Dark));
            skillHandlers.Add(7679, new FireArrow());
            skillHandlers.Add(7680, new WaterArrow());
            skillHandlers.Add(7681, new EarthArrow());
            skillHandlers.Add(7682, new WindArrow());
            skillHandlers.Add(7683, new LightArrow());
            skillHandlers.Add(7684, new DarkArrow());
            skillHandlers.Add(7685, new MobConArrow());
            skillHandlers.Add(7686, new MobChargeArrow());
            skillHandlers.Add(7687, new SumSlaveMob(26050003)); //ホウオウ
            skillHandlers.Add(7688, new MobTrDrop());
            skillHandlers.Add(7689, new MobConfCircle());
            skillHandlers.Add(7690, new SumMob(90010000));
            skillHandlers.Add(7691, new MobMedic());
            skillHandlers.Add(7692, new MobWindHighStorm2());
            skillHandlers.Add(7693, new MobElementLoad(7694));
            skillHandlers.Add(7694, new MobWindHighStorm2());
            skillHandlers.Add(7695, new MobElementRandcircle(7696, 5));
            skillHandlers.Add(7696, new MobWindRandcircleSeq2());
            skillHandlers.Add(7697, new MobElementRandcircle(7698, 5));
            skillHandlers.Add(7698, new MobWindCrosscircleSeq2());

            skillHandlers.Add(7706, new SumSlaveMob(10136901)); //魔狼
            skillHandlers.Add(7707, new SolidAura());
            //skillHandlers.Add(7709, new SkillDefinations.Sorcerer.Kyrie(SagaMap.Skill.SkillDefinations.Sorcerer.Kyrie.KyrieUser.Boss)); 
            skillHandlers.Add(7709, new MobComaStun());
            skillHandlers.Add(7710, new SumMob(90010000));
            skillHandlers.Add(7711, new MobSelfDarkHighStorm());
            skillHandlers.Add(4951, new MobSelfDarkHighStorm());
            skillHandlers.Add(7712, new SkillDefinations.Cabalist.DarkStorm(true));
            skillHandlers.Add(7713, new MobSelfMagStun());
            skillHandlers.Add(7714, new TrDrop2());
            skillHandlers.Add(7715, new MobHpPerDown());
            skillHandlers.Add(7716, new MobDropOut());
            skillHandlers.Add(7717, new SumSlaveMob(26180000)); //玉桂巴巴

            skillHandlers.Add(7719, new MobTalkSnmnpapa());
            skillHandlers.Add(7720, new SumSlaveMob(10120100)); //クリムゾンバウ
            skillHandlers.Add(7721, new SumSlaveMob(10210002)); //タランチュラ
            skillHandlers.Add(7722, new SumSlaveMob(10431000)); //キメラ(灰)
            skillHandlers.Add(7723, new SumSlaveMob(10680900)); //ゴースト(黒)
            skillHandlers.Add(7724, new SumSlaveMob(10251000)); //デカデス
            skillHandlers.Add(7725, new SumSlaveMob(10000006)); //デカプルル
            skillHandlers.Add(7726, new SumSlaveMob(10001005)); //メタリカ
            skillHandlers.Add(7727, new SumSlaveMob(10211400)); //夜叉蜘蛛
            skillHandlers.Add(7728, new ThunderBall(true));
            skillHandlers.Add(7729, new EarthBlast(true));
            skillHandlers.Add(7730, new FireBolt(true));
            skillHandlers.Add(7731, new StoneSkin(true));
            skillHandlers.Add(7732, new MobCancelChgstateAll());
            skillHandlers.Add(7733, new SumSlaveMob(10580500, 4)); //曼陀蘿芥末
            skillHandlers.Add(7734, new MobCircleSelfAtk());
            skillHandlers.Add(7735, new SumSlaveMob(10431400, 1)); //アルビノキメラ
            skillHandlers.Add(7736, new SumSlaveMob(10030901, 1)); //最強の魔獣
            skillHandlers.Add(7737, new MobCircleSelfAtk());
            skillHandlers.Add(7738, new EnergyShock(true));
            skillHandlers.Add(7739, new Concentricity(true));
            skillHandlers.Add(7740, new MobComboConShot());
            skillHandlers.Add(7741, new MobConShot());
            skillHandlers.Add(7742, new MobComboConAtk());
            skillHandlers.Add(7743, new MobConAtk());
            skillHandlers.Add(7744, new SumSlaveMob(10580500)); //曼陀蘿芥末
            //skillHandlers.Add(7745, new SkillDefinations.Enchanter.AcidMist());
            //skillHandlers.Add(7745, new SkillDefinations.Monster.AcidMistMobUse());
            MobskillHandlers.Add(7745, new AcidMistMobUse());
            //skillHandlers.Add(7746, new SkillDefinations.Monster.MobEarthDurable());
            MobskillHandlers.Add(7746, new MobEarthDurable());
            skillHandlers.Add(7747, new LifeSteal(true));
            skillHandlers.Add(7748, new MobChargeCircle());
            skillHandlers.Add(7749, new PetPlantPoison(true));
            skillHandlers.Add(7750, new MobStrVitAgiDownOne());
            skillHandlers.Add(7751, new MobAtkupSelf());
            skillHandlers.Add(7752, new MobDefUpSelf(true));
            skillHandlers.Add(7753, new SolidAura(SolidAura.KyrieUser.Mob));
            skillHandlers.Add(7754, new MobHolyfeather());
            skillHandlers.Add(7755, new MobSalvoFire());
            skillHandlers.Add(7756, new MobAmobm());
            skillHandlers.Add(7757, new SumMob(90010001));
            skillHandlers.Add(7758, new EnergyStorm(true));
            skillHandlers.Add(7759, new EnergyBlast(true));
            skillHandlers.Add(7760, new SumSlaveMob(10990000)); //バルル
            skillHandlers.Add(7761, new SumSlaveMob(10960000)); //野生德拉古
            skillHandlers.Add(7764, new MobIsSeN()); //怪物用一闪
            skillHandlers.Add(7766, new SumSlaveMob(19070500)); //ＤＥＭ－スナイパー

            skillHandlers.Add(7798, new Caputrue());
            skillHandlers.Add(7878, new NothingNess());

            skillHandlers.Add(7805, new SumSlaveMob(14160500)); //ポイズンジェル
            skillHandlers.Add(7806, new SumSlaveMob(14160000)); //バルーンジェル
            skillHandlers.Add(7807, new SumSlaveMob(10060600)); //エンジェルフィッシュ
            skillHandlers.Add(7808, new SumSlaveMob(10060200)); //スイムフィッシュ


            skillHandlers.Add(7810, new Abusoryutoteritori());
            skillHandlers.Add(7811, new SumSlaveMob(14110003, 8)); //スレイブドラゴン召喚

            MobskillHandlers.Add(7813, new CanisterShot(true));
            MobskillHandlers.Add(7815, new HellFire(true)); //DEM龙&恶魔龙用地狱之火
            skillHandlers.Add(7818, new SumSlaveMob(14320203)); //ＤＥＭ－クリンゲ召喚,现在DEM龙召唤49级的Link
            skillHandlers.Add(7819, new SumSlaveMob(14330500)); //ＤＥＭ－ゲヴェーア召喚

            skillHandlers.Add(7820, new SoulAttack());
            skillHandlers.Add(7821, new VolcanoHall());
            //skillHandlers.Add(7822, new SkillDefinations.BountyHunter.IsSeN(true));
            skillHandlers.Add(7825, new MobDevineBarrier()); //怪物用光界
            skillHandlers.Add(7830, new SumSlaveMob(14560900)); //アルルーン召喚
            skillHandlers.Add(7831, new Animadorein()); //怪物用核弹
            skillHandlers.Add(7832, new MobNoHeal()); //再生技能无效化
            skillHandlers.Add(7849, new MobElementBless(Elements.Fire)); //怪物用火祝
            skillHandlers.Add(7850, new MobElementBless(Elements.Wind)); //怪物用风祝
            skillHandlers.Add(7851, new MobElementBless(Elements.Water)); //怪物用水祝
            skillHandlers.Add(7852, new MobElementBless(Elements.Earth)); //怪物用地祝
            skillHandlers.Add(7853, new MobElementBless(Elements.Holy)); //怪物用光祝
            skillHandlers.Add(7854, new MobElementBless(Elements.Dark)); //怪物用暗祝
            skillHandlers.Add(7858, new ElementNull()); //属性攻击无效化
            skillHandlers.Add(7867, new SumSlaveMob(10270500)); //黄色天使之羽
            skillHandlers.Add(7874, new Roar()); //咆哮
            skillHandlers.Add(7875, new HumanSeeleGuerrilla()); //灵魂审判(5*5沉默+凭依解除)
            MobskillHandlers.Add(7859, new AlterFate()); //オールターフェイト 属性变化
            skillHandlers.Add(7883, new SumSlaveMob(14570002)); //圆桌骑士
            skillHandlers.Add(7884, new SumSlaveMob(14310105)); //自动医疗机召唤
            skillHandlers.Add(7885, new SumSlaveMob(14550901)); //末路幽灵
            skillHandlers.Add(7886, new SumSlaveMob(14170501)); //花纹蛇
            skillHandlers.Add(7887, new SumSlaveMob(14170502)); //花纹蛇
            skillHandlers.Add(7888, new SumSlaveMob(10140402)); //维德佛尔尼尔
            skillHandlers.Add(7889, new SumSlaveMob(10140403)); //维德佛尔尼尔
            MobskillHandlers.Add(7890, new Mutation()); //ミューテイション 属性变化
            skillHandlers.Add(7897, new SumSlaveMob(10960005)); //步行龙僵尸
            skillHandlers.Add(7898, new SumSlaveMob(10251000)); //梦魇
            skillHandlers.Add(7899, new SumSlaveMob(10470006)); //巴力西卜
            skillHandlers.Add(7900, new SumSlaveMob(14280001)); //玛尔斯
            skillHandlers.Add(7901, new SumSlaveMob(10221500)); //粉色鬼火
            skillHandlers.Add(7902, new SumSlaveMob(14171400)); //海蛇（白）
            skillHandlers.Add(7903, new SumSlaveMob(26130013)); //冰元素W
            skillHandlers.Add(7904, new SumSlaveMob(26130106)); //暗元素B
            skillHandlers.Add(7920, new SumSlaveMob(15620001)); //ポロマタンキクル(红跳跳)
            skillHandlers.Add(7987, new ZillionBladeMob()); //无尽之刃Mob
            skillHandlers.Add(8077, new BooooomMob()); //Mob自爆
            skillHandlers.Add(20151, new ConvolutionMob()); //大车轮Mob
            skillHandlers.Add(20155, new Corona()); //コロナMob
            skillHandlers.Add(25200, new VerbalMob()); //かまいたちMob
            skillHandlers.Add(8500, new MobHpHeal());
            skillHandlers.Add(8501, new MobBerserk());

            skillHandlers.Add(9000, new CswarSleep(true));
            skillHandlers.Add(9001, new CswarSleep(true));
            skillHandlers.Add(9002, new SumMob(26160003)); //サークリス

            skillHandlers.Add(9004, new AreaHeal(true));
            skillHandlers.Add(9106, new EventSelfDarkStorm(true));

            skillHandlers.Add(8444, new DeathSickle(true)); //死鎌乱舞 By Kk
            skillHandlers.Add(8476, new Bounce(true)); //セルフミラー Bt Kk

            #endregion

            #region Marionette

            skillHandlers.Add(5008, new HPRecovery());
            skillHandlers.Add(5009, new SPRecovery());
            skillHandlers.Add(5010, new MPRecovery());

            skillHandlers.Add(5507, new MExclamation());

            skillHandlers.Add(5513, new MBokeboke());

            skillHandlers.Add(5515, new MMirror());
            skillHandlers.Add(5516, new MMirrorSkill());

            skillHandlers.Add(5522, new MDarkCrosscircle());
            skillHandlers.Add(5523, new MDarkCrosscircleSeq());
            skillHandlers.Add(5524, new MCharge3()); //该技能不属于木偶师

            #endregion

            #region Event

            skillHandlers.Add(1500, new WeaCreUp());
            skillHandlers.Add(1501, new HitUpRateDown());

            skillHandlers.Add(1603, new Ryuugankaihou());
            skillHandlers.Add(1604, new RyuugankaihouShin());
            skillHandlers.Add(1605, new MagicSP());
            skillHandlers.Add(2072, new MoneyTime());
            skillHandlers.Add(2265, new NormalAttack());
            skillHandlers.Add(2457, new SymbolRepair());

            skillHandlers.Add(3067, new PowerUP());
            skillHandlers.Add(3069, new HPUP());
            skillHandlers.Add(3071, new SpeedUP());
            skillHandlers.Add(3145, new MagicUP());
            skillHandlers.Add(3269, new ChgTrance());
            skillHandlers.Add(5509, new Colder());
            skillHandlers.Add(5510, new ConflictKick());

            skillHandlers.Add(6415, new MoveUp2());
            skillHandlers.Add(6428, new MoveUp3());
            skillHandlers.Add(6451, new MoveUp5()); //紫火
            skillHandlers.Add(9100, new MiniMum());
            skillHandlers.Add(9101, new MaxiMum());
            skillHandlers.Add(9102, new EventCampfire());
            skillHandlers.Add(9103, new Invisible());
            skillHandlers.Add(9105, new EventCampfire());
            skillHandlers.Add(9108, new Dango());
            skillHandlers.Add(9109, new EventCampfire());
            skillHandlers.Add(9114, new Invisible());
            skillHandlers.Add(9117, new ExpUp());
            skillHandlers.Add(9126, new EventCampfire());
            skillHandlers.Add(9127, new EventCampfire());
            skillHandlers.Add(9128, new Invisible());
            skillHandlers.Add(9129, new SumMobCastSkill(19010001, 9130));
            skillHandlers.Add(9130, new HpRecoveryMax());
            skillHandlers.Add(9131, new SumMobCastSkill(19010002, 9132));
            skillHandlers.Add(9132, new HpRecoveryMax());
            skillHandlers.Add(9133, new SumMobCastSkill(19010003, 9134));
            skillHandlers.Add(9134, new HpRecoveryMax());

            skillHandlers.Add(9140, new SumMobCastSkill(19010008, 9143, 50, 9146, 50));
            skillHandlers.Add(9141, new SumMobCastSkill(19010009, 9144, 50, 9147, 50));
            skillHandlers.Add(9142, new SumMobCastSkill(19010010, 9145, 50, 9148, 50));
            skillHandlers.Add(9143, new DefMdefUp());
            skillHandlers.Add(9144, new DefMdefUp());
            skillHandlers.Add(9145, new DefMdefUp());
            skillHandlers.Add(9146, new DefMdefUp());
            skillHandlers.Add(9147, new DefMdefUp());
            skillHandlers.Add(9148, new DefMdefUp());


            skillHandlers.Add(9139, new EventCampfire());

            skillHandlers.Add(9151, new ILoveYou());
            skillHandlers.Add(9152, new HpRecoveryMax());
            skillHandlers.Add(9153, new HpRecoveryMax());
            skillHandlers.Add(9154, new HpRecoveryMax());
            skillHandlers.Add(9155, new Healing());
            skillHandlers.Add(9157, new EventCampfire());
            skillHandlers.Add(9162, new Healing());
            skillHandlers.Add(9163, new EventCampfire());
            skillHandlers.Add(9174, new EventCampfire());
            skillHandlers.Add(9178, new EventCampfire());
            skillHandlers.Add(9182, new EventCampfire());
            skillHandlers.Add(9185, new EventCampfire());
            skillHandlers.Add(9190, new SumMob(30740000));
            skillHandlers.Add(9191, new SumMobCastSkill(30750000, 9192, 90, 9193, 10));
            skillHandlers.Add(9192, new WeepingWillow1());
            skillHandlers.Add(9193, new WeepingWillow2());

            skillHandlers.Add(9197, new Invisible());
            skillHandlers.Add(5520, new DarkSun());

            skillHandlers.Add(9208, new SumMobCastSkill(19010028, 9209));
            skillHandlers.Add(9209, new HpRecoveryMax());

            skillHandlers.Add(9219, new MoveUp4());
            skillHandlers.Add(9220, new RiceSeed());
            skillHandlers.Add(9221, new PanTick());

            skillHandlers.Add(9223, new Gravity());
            skillHandlers.Add(9224, new Kyrie());
            skillHandlers.Add(9225, new SkillDefinations.Event.AReflection());
            skillHandlers.Add(9226, new STR_VIT_AGI_UP());
            skillHandlers.Add(9227, new MAG_INT_DEX_UP());
            skillHandlers.Add(9228, new SkillDefinations.Event.AreaHeal());

            skillHandlers.Add(10500, new HerosProtection());

            #endregion

            #region Swordman

            skillHandlers.Add(2005, new SwordCancel());
            skillHandlers.Add(2100, new Parry());
            skillHandlers.Add(2101, new Counter());
            skillHandlers.Add(2102, new Feint());
            skillHandlers.Add(2107, new Provocation());
            skillHandlers.Add(2111, new BanishBlow());
            skillHandlers.Add(2114, new SlowBlow());
            skillHandlers.Add(2120, new Charge());
            skillHandlers.Add(2117, new CutDown());
            skillHandlers.Add(2115, new SkillDefinations.Swordman.Iai());
            skillHandlers.Add(2201, new Iai2());
            skillHandlers.Add(2202, new Iai3());

            #endregion

            #region BladeMaster

            skillHandlers.Add(2134, new aEarthAngry());
            skillHandlers.Add(2231, new aWoodHack());
            skillHandlers.Add(2232, new aFalconLongSword());
            skillHandlers.Add(2233, new aMistBehead());
            skillHandlers.Add(2234, new aAnimalCrushing());
            skillHandlers.Add(2116, new StormSword());
            skillHandlers.Add(2235, new aHundredSpriteCry());
            skillHandlers.Add(700, new P_BERSERK());
            skillHandlers.Add(2066, new PetMeditatioon());
            skillHandlers.Add(2109, new PetWarCry());
            skillHandlers.Add(2236, new AtkFly());
            skillHandlers.Add(2237, new SwordEaseSp());
            skillHandlers.Add(2238, new EaseCt());
            skillHandlers.Add(2379, new DoubleCutDown());
            skillHandlers.Add(2380, new DoubleCutDownSeq());

            #endregion

            #region BountyHunter

            skillHandlers.Add(2272, new ArmSlash());
            skillHandlers.Add(2271, new BodySlash());
            skillHandlers.Add(2273, new ChestSlash());
            skillHandlers.Add(2122, new MiNeUChi());
            skillHandlers.Add(2274, new ShieldSlash());
            skillHandlers.Add(2269, new DefUpAvoDown());
            skillHandlers.Add(2268, new AtkUpHitDown());
            skillHandlers.Add(2352, new BeatUp());
            skillHandlers.Add(2401, new MuSoU());
            skillHandlers.Add(2402, new MuSoUSEQ());
            skillHandlers.Add(2396, new SwordAssail());
            skillHandlers.Add(2397, new SwordAssailSEQ());
            skillHandlers.Add(2136, new AShiBaRaI());
            skillHandlers.Add(956, new ConSword());
            //skillHandlers.Add(400, new SkillDefinations.Global.SomeKindDamUp("HumDamUp", SagaDB.Mob.MobType.HUMAN, SagaDB.Mob.MobType.HUMAN_BOSS, SagaDB.Mob.MobType.HUMAN_BOSS_SKILL, SagaDB.Mob.MobType.HUMAN_RIDE, SagaDB.Mob.MobType.HUMAN_SKILL));
            skillHandlers.Add(2275, new PartsSlash());
            skillHandlers.Add(2355, new StrikeBlow());
            skillHandlers.Add(2353, new SolidBody());
            skillHandlers.Add(2400, new IsSeN());
            skillHandlers.Add(2179, new EdgedSlash());
            skillHandlers.Add(2270, new ComboIai());

            #endregion

            #region Gladiator

            skillHandlers.Add(982, new SwordMaster());
            skillHandlers.Add(3350, new HPCommunion());
            skillHandlers.Add(3362, new Gladiator()); //11月23日增加，LV10习得
            skillHandlers.Add(1100, new Volunteers()); //11月23日增加，LV13习得
            skillHandlers.Add(2484, new Ekuviri());
            skillHandlers.Add(2498, new DevilStance());
            skillHandlers.Add(2503, new Convolution());
            skillHandlers.Add(3391, new Pressure()); //11月23日增加，LV30习得
            skillHandlers.Add(1113, new Sewaibu()); //11月23日增加，LV35习得
            skillHandlers.Add(2528, new Disarm()); //11月23日增加，LV40习得
            skillHandlers.Add(2527, new SpeedHit()); //11月23日增加，LV45习得
            skillHandlers.Add(1117, new KenSei()); //16.05.03增加,LV47
            skillHandlers.Add(2534, new ZillionBlade()); //16.05.03增加,LV50

            #endregion

            #region Scout

            skillHandlers.Add(2001, new SkillDefinations.Scout.CriUp());
            skillHandlers.Add(2008, new ShortSwordCancel());
            skillHandlers.Add(2139, new ConThrust());
            skillHandlers.Add(2143, new SummerSaltKick());
            skillHandlers.Add(2133, new WalkAround());
            skillHandlers.Add(110, new ShortSwordMastery());
            skillHandlers.Add(2004, new AvoidMeleeUp());
            skillHandlers.Add(112, new ThrowMastery());
            skillHandlers.Add(2127, new ConThrow());
            skillHandlers.Add(908, new ThrowRangeUp());

            #endregion

            #region Assassin

            skillHandlers.Add(2045, new PoisonReate());
            skillHandlers.Add(2046, new PosionReate2());
            skillHandlers.Add(2047, new PoisonReate3());
            skillHandlers.Add(2069, new Concentricity());
            skillHandlers.Add(947, new ConClaw());
            skillHandlers.Add(2062, new WillPower());
            skillHandlers.Add(910, new PoisonRegiUp());
            skillHandlers.Add(2250, new UTuSeMi());
            skillHandlers.Add(2044, new AppliePoison());
            skillHandlers.Add(2142, new ScatterPoison());
            skillHandlers.Add(920, new PoisonRateUp());
            skillHandlers.Add(2251, new EventSumNinJa());

            #endregion

            #region Command

            skillHandlers.Add(127, new HandGunDamUp());
            skillHandlers.Add(2137, new Tackle());
            skillHandlers.Add(125, new MartialArtDamUp());
            skillHandlers.Add(2141, new SkillDefinations.Command.Rush());
            skillHandlers.Add(2282, new FlashHandGrenade());
            skillHandlers.Add(2362, new SetBomb());
            skillHandlers.Add(2378, new SetBomb2());
            skillHandlers.Add(2359, new UpperCut());
            skillHandlers.Add(2413, new WildDance());
            skillHandlers.Add(2414, new WildDance2());
            skillHandlers.Add(3095, new LandTrap());
            skillHandlers.Add(2280, new FullNelson());
            skillHandlers.Add(2281, new Combination());
            skillHandlers.Add(3131, new ChokingGas());
            skillHandlers.Add(2283, new Sliding());
            skillHandlers.Add(2284, new ClayMore());
            skillHandlers.Add(2361, new SealHMSp());
            skillHandlers.Add(2409, new RushBom());
            skillHandlers.Add(2410, new RushBomSeq());
            skillHandlers.Add(2411, new RushBomSeq2());
            skillHandlers.Add(2408, new SumCommand());
            //skillHandlers.Add(401, new SkillDefinations.Command.HumHitUp());

            #endregion

            #region Wizard

            skillHandlers.Add(3001, new EnergyOne());
            skillHandlers.Add(3002, new EnergyGroove());
            skillHandlers.Add(3005, new Decoy());
            skillHandlers.Add(3113, new EnergyShield());
            skillHandlers.Add(3279, new EnergyShield());
            skillHandlers.Add(3114, new MagicShield());
            skillHandlers.Add(3280, new MagicShield());
            skillHandlers.Add(3123, new EnergyShock());
            skillHandlers.Add(3125, new DancingSword());
            skillHandlers.Add(3124, new EnergySpear());
            skillHandlers.Add(3127, new EnergyBlast());
            skillHandlers.Add(3135, new SkillDefinations.Wizard.MagPoison());
            skillHandlers.Add(3136, new MagStone());
            skillHandlers.Add(3139, new MagSilence());
            skillHandlers.Add(801, new MaGaNiInfo());
            skillHandlers.Add(3101, new Heating());
            skillHandlers.Add(3281, new MobEnergyShock());
            skillHandlers.Add(3100, new IntenseHeatSheld());
            skillHandlers.Add(3033, new Aqualung());
            skillHandlers.Add(3003, new EnergyWall());
            skillHandlers.Add(503, new ManDamUp());
            skillHandlers.Add(504, new ManHitUp());
            skillHandlers.Add(505, new ManAvoUp());
            //skillHandlers.Add(400, new SkillDefinations.Wizard.WandMaster());
            //Wrong SkillID !! KK 2018/4/9
            skillHandlers.Add(401, new EnergyExcess());

            #endregion

            #region Sorcerer

            skillHandlers.Add(3126, new LivingSword());
            skillHandlers.Add(3300, new DevineBarrier());
            skillHandlers.Add(3253, new Teleport());
            skillHandlers.Add(3097, new Invisible());
            skillHandlers.Add(3275, new EnergyBarrier());
            skillHandlers.Add(3276, new MagicBarrier());
            skillHandlers.Add(3256, new Clutter());
            skillHandlers.Add(3255, new StrVitAgiDownOne());
            skillHandlers.Add(3298, new EnergyBarn());
            skillHandlers.Add(3299, new EnergyBarnSEQ());
            skillHandlers.Add(3158, new Desist());
            skillHandlers.Add(3254, new SolidAura());
            skillHandlers.Add(2208, new MaganiAnalysis());
            skillHandlers.Add(3171, new MobInvisibleBreak());
            skillHandlers.Add(3098, new MobInvisibleBreak());
            skillHandlers.Add(3004, new WallSweep());
            skillHandlers.Add(3094, new HexaGram());
            skillHandlers.Add(2252, new OverWork());

            #endregion

            #region Vates

            //skillHandlers.Add(3111, new SkillDefinations.Vates.HolyBlessing());
            skillHandlers.Add(3111, new ElementBless(Elements.Holy));
            //skillHandlers.Add(3080, new SkillDefinations.Vates.HolyHealing());
            skillHandlers.Add(3080, new ElementCircle(Elements.Holy)); //光之领域
            skillHandlers.Add(3054, new Healing());
            skillHandlers.Add(3165, new SmallHealing());
            skillHandlers.Add(3055, new Resurrection());
            skillHandlers.Add(3073, new SkillDefinations.Vates.LightOne());
            //skillHandlers.Add(3075, new SkillDefinations.Vates.HolyWeapon());
            //skillHandlers.Add(3076, new SkillDefinations.Vates.HolyShield());
            skillHandlers.Add(3075, new ElementWeapon(Elements.Holy));
            skillHandlers.Add(3076, new ElementShield(Elements.Holy));
            skillHandlers.Add(3082, new HolyGroove());
            skillHandlers.Add(3066, new CureStatus("Sleep"));
            skillHandlers.Add(3060, new CureStatus("Poison"));
            skillHandlers.Add(3058, new CureStatus("Stun"));
            skillHandlers.Add(3062, new CureStatus("Silence"));
            skillHandlers.Add(3150, new CureStatus("Stone"));
            skillHandlers.Add(3064, new CureStatus("Confuse"));
            skillHandlers.Add(3152, new CureStatus("鈍足"));
            skillHandlers.Add(3154, new CureStatus("Frosen"));
            skillHandlers.Add(803, new UndeadInfo());
            skillHandlers.Add(3065, new StatusRegi("Sleep"));
            skillHandlers.Add(3059, new StatusRegi("Poison"));
            skillHandlers.Add(3057, new StatusRegi("Stun"));
            skillHandlers.Add(3061, new StatusRegi("Silence"));
            skillHandlers.Add(3149, new StatusRegi("Stone"));
            skillHandlers.Add(3063, new StatusRegi("Confuse"));
            skillHandlers.Add(3151, new StatusRegi("鈍足"));
            skillHandlers.Add(3153, new StatusRegi("Frosen"));
            skillHandlers.Add(3078, new TurnUndead());

            #endregion

            #region Shaman

            skillHandlers.Add(3006, new FireBolt());
            //skillHandlers.Add(3007, new SkillDefinations.Shaman.FireShield());
            //skillHandlers.Add(3008, new SkillDefinations.Shaman.FireWeapon());
            skillHandlers.Add(3007, new ElementShield(Elements.Fire));
            skillHandlers.Add(3008, new ElementWeapon(Elements.Fire));
            skillHandlers.Add(3009, new FireBlast());
            skillHandlers.Add(3029, new SkillDefinations.Shaman.IceArrow());
            //skillHandlers.Add(3030, new SkillDefinations.Shaman.WaterShield());
            //skillHandlers.Add(3031, new SkillDefinations.Shaman.WaterWeapon());
            skillHandlers.Add(3030, new ElementShield(Elements.Water));
            skillHandlers.Add(3031, new ElementWeapon(Elements.Water));
            skillHandlers.Add(3032, new ColdBlast());
            skillHandlers.Add(3041, new LandKlug());
            //skillHandlers.Add(3042, new SkillDefinations.Shaman.EarthShield());
            //skillHandlers.Add(3043, new SkillDefinations.Shaman.EarthWeapon());
            skillHandlers.Add(3042, new ElementShield(Elements.Earth));
            skillHandlers.Add(3043, new ElementWeapon(Elements.Earth));
            skillHandlers.Add(3044, new EarthBlast());
            skillHandlers.Add(3017, new ThunderBall());
            //skillHandlers.Add(3018, new SkillDefinations.Shaman.WindShield());
            //skillHandlers.Add(3019, new SkillDefinations.Shaman.WindWeapon());
            skillHandlers.Add(3018, new ElementShield(Elements.Wind));
            skillHandlers.Add(3019, new ElementWeapon(Elements.Wind));
            skillHandlers.Add(3020, new LightningBlast());
            skillHandlers.Add(3011, new FireWall());
            skillHandlers.Add(3047, new StoneWall());
            skillHandlers.Add(802, new ElementIInfo());
            skillHandlers.Add(3000, new SenseElement());
            skillHandlers.Add(3162, new PrayerToTheElf());

            #endregion

            #region Elementaler

            skillHandlers.Add(3016, new FireGroove());
            skillHandlers.Add(3028, new WindGroove());
            skillHandlers.Add(3040, new SkillDefinations.Elementaler.WaterGroove());
            skillHandlers.Add(3053, new EarthGroove());
            skillHandlers.Add(3265, new LavaFlow());
            skillHandlers.Add(3036, new ElementStorm(Elements.Water));
            skillHandlers.Add(3025, new ElementStorm(Elements.Wind));
            skillHandlers.Add(3049, new ElementStorm(Elements.Earth));
            skillHandlers.Add(3013, new ElementStorm(Elements.Fire));
            skillHandlers.Add(3261, new ChainLightning());
            skillHandlers.Add(3260, new CatlingGun());
            skillHandlers.Add(3262, new WaterNum());
            skillHandlers.Add(3263, new EarthNum());
            skillHandlers.Add(3264, new IcicleTempest());
            skillHandlers.Add(3311, new SpellCancel());
            skillHandlers.Add(3159, new Zen());
            skillHandlers.Add(2209, new ElementAnalysis());
            skillHandlers.Add(3301, new AquaWave());
            skillHandlers.Add(3306, new CycloneGrooveEarth());
            skillHandlers.Add(939, new ElementLimitUp(Elements.Earth)); //大地守護
            skillHandlers.Add(936, new ElementLimitUp(Elements.Fire)); //火焰守護
            skillHandlers.Add(937, new ElementLimitUp(Elements.Water)); //水靈守護
            skillHandlers.Add(938, new ElementLimitUp(Elements.Wind)); //神風守護

            #endregion

            #region Enchanter

            skillHandlers.Add(3318, new GravityFall());
            skillHandlers.Add(3319, new ElementalWrath());
            skillHandlers.Add(3294, new SpeedEnchant());
            skillHandlers.Add(3295, new AtkUp_DefUp_SpdDown_AvoDown());
            skillHandlers.Add(2305, new SoulOfEarth());
            skillHandlers.Add(2304, new SoulOfWind());
            skillHandlers.Add(2303, new SoulOfWater());
            skillHandlers.Add(2302, new SoulOfFire());
            skillHandlers.Add(3046, new PoisonMash());
            skillHandlers.Add(3155, new Bind());
            skillHandlers.Add(3052, new AcidMist());
            skillHandlers.Add(3010, new FirePillar());
            skillHandlers.Add(3048, new ElementCircle(Elements.Earth)); //大地結界
            skillHandlers.Add(3012, new ElementCircle(Elements.Fire)); //火焰結界
            skillHandlers.Add(3035, new ElementCircle(Elements.Water)); //寒冰結界
            skillHandlers.Add(3024, new ElementCircle(Elements.Wind)); //神風結界
            skillHandlers.Add(3296, new ElementBall());
            skillHandlers.Add(3317, new EnchantWeapon());
            skillHandlers.Add(3110, new ElementBless(Elements.Earth)); //大地祝福
            skillHandlers.Add(3107, new ElementBless(Elements.Fire)); //火焰祝福
            skillHandlers.Add(3109, new ElementBless(Elements.Water)); //寒氣祝福
            skillHandlers.Add(3108, new ElementBless(Elements.Wind)); //神風祝福

            #endregion

            #region Acher

            skillHandlers.Add(2050, new BowCancel());
            skillHandlers.Add(2128, new ConArrow());

            #endregion

            #region Warlock

            skillHandlers.Add(3083, new BlackWidow());
            skillHandlers.Add(3085, new SkillDefinations.Warlock.ShadowBlast());
            //skillHandlers.Add(3088, new SkillDefinations.Warlock.DarkWeapon());
            skillHandlers.Add(3088, new ElementWeapon(Elements.Dark));
            skillHandlers.Add(3093, new DarkGroove());
            //skillHandlers.Add(3133, new SkillDefinations.Warlock.DarkShield());
            skillHandlers.Add(3133, new ElementShield(Elements.Dark));
            skillHandlers.Add(3134, new ChaosWidow());
            skillHandlers.Add(3140, new SkillDefinations.Warlock.MagSlow());
            skillHandlers.Add(3141, new MagConfuse());
            skillHandlers.Add(3142, new MagFreeze());
            skillHandlers.Add(3143, new MagStun());
            skillHandlers.Add(3112, new ElementBless(Elements.Dark)); //黑暗祝福
            skillHandlers.Add(941, new ElementLimitUp(Elements.Dark)); //黑暗守護
            skillHandlers.Add(8456, new SuckBlood());

            #endregion

            #region Cabalist

            skillHandlers.Add(2229, new GrimReaper());
            skillHandlers.Add(2230, new SoulSteal());
            skillHandlers.Add(3092, new ElementCircle(Elements.Dark)); //暗黑結界（ダークパワーサークル）
            skillHandlers.Add(3087, new Fanaticism());
            skillHandlers.Add(3089, new SkillDefinations.Cabalist.DarkStorm());
            skillHandlers.Add(3274, new MoveDownCircle());
            skillHandlers.Add(3021, new SleepCloud());
            skillHandlers.Add(949, new AllRateUp());
            skillHandlers.Add(3167, new DarkChopMark());
            skillHandlers.Add(3166, new ChopMark());
            skillHandlers.Add(3270, new HitAndAway());
            skillHandlers.Add(3273, new StoneSkin());
            skillHandlers.Add(3272, new RandMark());
            skillHandlers.Add(3309, new ChgstRand());
            skillHandlers.Add(3310, new EventSelfDarkStorm());
            skillHandlers.Add(3346, new Sacrifice());
            skillHandlers.Add(10000, new EffDarkChopMark());

            #endregion

            #region Fencer

            skillHandlers.Add(2007, new SpearCancel());
            skillHandlers.Add(2003, new MobDefUpSelf());
            skillHandlers.Add(106, new GuardUp());
            skillHandlers.Add(2064, new AstuteStab());

            #endregion

            #region Knight

            skillHandlers.Add(2123, new SkillDefinations.Knight.ShockWave());
            skillHandlers.Add(2247, new AtkUnDead());
            skillHandlers.Add(946, new ConSpear());
            skillHandlers.Add(2065, new AstuteBlow());
            skillHandlers.Add(934, new ElementAddUp(Elements.Holy, "LightAddUp"));
            skillHandlers.Add(2041, new DifrectArrow());
            skillHandlers.Add(2063, new AstuteSlash());
            skillHandlers.Add(2245, new CutDownSpear());
            skillHandlers.Add(4025, new DJoint());
            skillHandlers.Add(4029, new AProtect());
            skillHandlers.Add(2248, new HoldShield());
            skillHandlers.Add(2246, new HitRow());
            skillHandlers.Add(2125, new Valkyrie());
            skillHandlers.Add(3251, new VicariouslyResu());
            skillHandlers.Add(2381, new DirlineRandSeq());
            skillHandlers.Add(2382, new DirlineRandSeq2());
            skillHandlers.Add(2061, new Revive());

            #endregion

            #region Tatarabe

            skillHandlers.Add(2009, new Synthese());
            skillHandlers.Add(2051, new Synthese());
            skillHandlers.Add(2083, new Synthese());
            skillHandlers.Add(2185, new AtkRow());
            skillHandlers.Add(800, new RockInfo());
            skillHandlers.Add(2200, new EventCampfire());
            skillHandlers.Add(2071, new PosturetorToise());
            skillHandlers.Add(905, new GoRiKi());
            skillHandlers.Add(2177, new StoneThrow());
            skillHandlers.Add(2135, new ThrowDirt());

            #endregion

            #region Blacksmith

            skillHandlers.Add(2010, new Synthese());
            skillHandlers.Add(2017, new Synthese());
            skillHandlers.Add(3342, new FrameHart());
            skillHandlers.Add(2224, new RockCrash());
            skillHandlers.Add(2198, new FirstAid());
            skillHandlers.Add(2194, new KnifeGrinder());
            skillHandlers.Add(2388, new ThrowNugget());
            skillHandlers.Add(2387, new EearthCrash());
            skillHandlers.Add(6050, new PetAttack());
            skillHandlers.Add(6051, new PetBack());
            skillHandlers.Add(2207, new RockAnalysis());
            skillHandlers.Add(6102, new PetCastSkill(6103, "MACHINE"));
            skillHandlers.Add(6103, new PetMacAtk());
            skillHandlers.Add(6101, new PetMacLHitUp());
            skillHandlers.Add(930, new FireAddup());
            skillHandlers.Add(6104, new PetCastSkill(6105, "MACHINE"));
            skillHandlers.Add(6105, new PetMacCircleAtk());
            skillHandlers.Add(2262, new SupportRockInfo());
            skillHandlers.Add(2253, new GuideRock());
            skillHandlers.Add(2261, new DurDownCancel());
            skillHandlers.Add(2395, new Balls());
            skillHandlers.Add(942, new BoostPower());
            skillHandlers.Add(409, new StoDamUp());
            skillHandlers.Add(410, new StoHitUp());
            skillHandlers.Add(411, new StoAvoUp());

            #endregion

            #region Machinery

            skillHandlers.Add(2039, new Synthese());
            skillHandlers.Add(809, new MachineInfo());
            skillHandlers.Add(132, new RobotDamUp());
            skillHandlers.Add(970, new RobotRecUp());
            skillHandlers.Add(964, new RobotHpUp());
            skillHandlers.Add(2326, new RobotAmobm());
            skillHandlers.Add(968, new RobotHitUp());
            skillHandlers.Add(966, new SkillDefinations.Machinery.RobotDefUp());
            skillHandlers.Add(2323, new RobotChaff());
            skillHandlers.Add(969, new RobotAvoUp());
            skillHandlers.Add(965, new SkillDefinations.Machinery.RobotAtkUp());
            skillHandlers.Add(2324, new MirrorSkill());
            skillHandlers.Add(2325, new RobotTeleport());
            skillHandlers.Add(2322, new RobotBerserk());
            skillHandlers.Add(2368, new RobotChillLaser());
            skillHandlers.Add(2327, new RobotSalvoFire());
            skillHandlers.Add(2369, new RobotEcm());
            skillHandlers.Add(2422, new RobotFireRadiation());
            skillHandlers.Add(2424, new RobotOverTune());
            skillHandlers.Add(2423, new RobotSparkBall());
            skillHandlers.Add(2425, new RobotLovageCannon());
            skillHandlers.Add(506, new MciDamUp());
            skillHandlers.Add(507, new MciHitUp());
            skillHandlers.Add(508, new MciAvoUp());

            #endregion

            #region Farmasist

            skillHandlers.Add(2020, new Synthese());
            skillHandlers.Add(2034, new Synthese());
            skillHandlers.Add(2040, new Synthese());
            skillHandlers.Add(2054, new Synthese());
            skillHandlers.Add(2085, new Synthese());
            skillHandlers.Add(2086, new Synthese());
            skillHandlers.Add(2089, new Synthese());
            skillHandlers.Add(3128, new Cultivation());
            skillHandlers.Add(807, new PlantInfo());
            skillHandlers.Add(804, new TreeInfo());
            skillHandlers.Add(2169, new GrassTrap());
            skillHandlers.Add(2170, new PitTrap());
            skillHandlers.Add(2196, new HealingTree());

            #endregion

            #region Alchemist

            skillHandlers.Add(2022, new Synthese());
            skillHandlers.Add(2118, new SkillDefinations.Alchemist.Phalanx());
            skillHandlers.Add(3096, new DelayTrap());
            skillHandlers.Add(2389, new DustExplosion());
            skillHandlers.Add(2214, new PlantAnalysis());
            skillHandlers.Add(954, new FoodThrow());
            skillHandlers.Add(909, new PotionFighter());
            skillHandlers.Add(406, new PlaDamUp());
            skillHandlers.Add(407, new PlaHitUp());
            skillHandlers.Add(408, new PlaAvoUp());
            skillHandlers.Add(6202, new PetCastSkill(6203, "PLANT"));
            skillHandlers.Add(6203, new PetPlantAtk());
            skillHandlers.Add(6206, new PetCastSkill(6207, "PLANT"));
            skillHandlers.Add(6207, new PetPlantDefupSelf());
            skillHandlers.Add(6204, new PetCastSkill(6205, "PLANT"));
            skillHandlers.Add(6205, new PetPlantHealing());
            skillHandlers.Add(2263, new SupportPlantInfo());
            skillHandlers.Add(943, new BoostMagic());
            skillHandlers.Add(2390, new ThrowChemical());
            skillHandlers.Add(3343, new SumChemicalPlant());
            skillHandlers.Add(3344, new SumChemicalPlant2());
            skillHandlers.Add(6200, new PetCastSkill(6201, "PLANT"));
            skillHandlers.Add(6201, new PetPlantPoison());
            skillHandlers.Add(2211, new TreeAnalysis());
            skillHandlers.Add(4028, new Super_A_T_PJoint()); //强力援手,但技能内容空白

            #endregion

            #region Marionest

            skillHandlers.Add(2038, new Synthese());
            skillHandlers.Add(133, new MarioDamUp());
            skillHandlers.Add(2328, new MarioCtrl());
            skillHandlers.Add(967, new MarioCtrlMove());
            skillHandlers.Add(2329, new MarioOver());
            skillHandlers.Add(2331, new EnemyCharming());
            skillHandlers.Add(2335, new MarioEarthWater());
            skillHandlers.Add(2334, new MarioWindEarth());
            skillHandlers.Add(2333, new MarioFireWind());
            skillHandlers.Add(2332, new MarioWaterFire());
            skillHandlers.Add(2371, new Puppet());
            skillHandlers.Add(976, new MarioTimeUp());
            skillHandlers.Add(2370, new MarionetteHarmony()); //2-2JOB36,大概是个错误的实装……
            skillHandlers.Add(980, new ChangeMarionette());
            skillHandlers.Add(3333, new MarioCancel());
            skillHandlers.Add(981, new MariostateUp());
            skillHandlers.Add(3334, new SumMario(26040001, 3335));
            skillHandlers.Add(3335, new SumMarioCont(Elements.Fire));
            skillHandlers.Add(3336, new SumMario(26100009, 3337));
            skillHandlers.Add(3337, new SumMarioCont(Elements.Water));
            skillHandlers.Add(3338, new SumMario(26100009, 3339));
            skillHandlers.Add(3339, new SumMarioCont(Elements.Wind));
            skillHandlers.Add(3340, new SumMario(26070003, 3341));
            skillHandlers.Add(3341, new SumMarioCont(Elements.Earth));

            #endregion

            #region Ranger

            skillHandlers.Add(2088, new Synthese());
            skillHandlers.Add(713, new Bivouac());
            skillHandlers.Add(805, new InsectInfo());
            skillHandlers.Add(806, new BirdInfo());
            skillHandlers.Add(808, new AnimalInfo());
            skillHandlers.Add(812, new WataniInfo());
            skillHandlers.Add(816, new TreasureInfo());
            skillHandlers.Add(2197, new CswarSleep());
            skillHandlers.Add(2103, new Unlock());
            skillHandlers.Add(403, new AniDamUp());
            skillHandlers.Add(404, new AniHitUp());
            skillHandlers.Add(405, new AniAvoUp());
            skillHandlers.Add(415, new WanDamUp());
            skillHandlers.Add(416, new WanHitUp());
            skillHandlers.Add(417, new WanAvoUp());

            #endregion

            #region Druid

            skillHandlers.Add(3146, new SkillDefinations.Druid.CureAll());
            skillHandlers.Add(3307, new RegiAllUp());
            skillHandlers.Add(3308, new AreaHeal());
            skillHandlers.Add(3257, new SkillDefinations.Druid.STR_VIT_AGI_UP());
            skillHandlers.Add(3258, new SkillDefinations.Druid.MAG_INT_DEX_UP());
            skillHandlers.Add(3056, new HolyFeather());
            skillHandlers.Add(3164, new FlashLight());
            //skillHandlers.Add(3080, new SkillDefinations.Enchanter.ElementCircle(Elements.Holy));//熾天使之翼（ホーリーパワーサークル
            skillHandlers.Add(3163, new SunLightShower());
            skillHandlers.Add(3268, new CriAvoDownOne());
            skillHandlers.Add(3266, new LightHigeCircle());
            skillHandlers.Add(3118, new Seal());
            skillHandlers.Add(2210, new UndeadAnalysis());
            skillHandlers.Add(3267, new UndeadMdefDownOne());
            skillHandlers.Add(3077, new ClairvoYance());
            skillHandlers.Add(3119, new SealMagic());
            skillHandlers.Add(950, new TranceSpdUp());
            skillHandlers.Add(940, new ElementLimitUp(Elements.Holy)); //光明守護
            skillHandlers.Add(509, new UndDamUp());
            skillHandlers.Add(510, new UndHitUp());
            skillHandlers.Add(511, new UndAvoUp());
            skillHandlers.Add(3345, new AllHealing());

            #endregion

            #region Bard

            skillHandlers.Add(2310, new Samba());
            skillHandlers.Add(3323, new DeadMarch());
            skillHandlers.Add(2313, new Classic());
            skillHandlers.Add(2311, new HeavyMetal());
            skillHandlers.Add(2312, new RockAndRoll());
            skillHandlers.Add(2309, new Transformer());
            skillHandlers.Add(2367, new MusicalBlow());
            skillHandlers.Add(2366, new Shout());
            skillHandlers.Add(2365, new Relaxation());
            skillHandlers.Add(2315, new BardSession());
            skillHandlers.Add(3321, new ORaToRiO());
            skillHandlers.Add(2308, new PopMusic());
            skillHandlers.Add(2307, new Fusion());
            skillHandlers.Add(2306, new ChangeMusic());
            skillHandlers.Add(131, new MusicalDamUp());
            skillHandlers.Add(2314, new Requiem());
            skillHandlers.Add(3322, new AttractMarch());
            skillHandlers.Add(3320, new LoudSong());

            #endregion

            #region Sage

            skillHandlers.Add(2030, new Synthese());
            skillHandlers.Add(2031, new Synthese());
            skillHandlers.Add(2032, new Synthese());
            skillHandlers.Add(2033, new Synthese());
            skillHandlers.Add(2296, new IntelRides());
            skillHandlers.Add(3169, new EnergyStorm());
            skillHandlers.Add(2330, new EnergyFreak());
            skillHandlers.Add(3291, new EnergyFlare());
            skillHandlers.Add(3292, new ChgstBlock());
            skillHandlers.Add(3312, new LuminaryNova());
            skillHandlers.Add(3315, new LastInQuest());
            skillHandlers.Add(3313, new AReflection());
            skillHandlers.Add(130, new ReadDamup());
            skillHandlers.Add(2295, new StaffCtrl());
            skillHandlers.Add(2297, new Provide());
            skillHandlers.Add(2294, new MonsterSketch());
            skillHandlers.Add(3293, new MagHitUpCircle());
            skillHandlers.Add(3314, new SumDop());

            #endregion

            #region Necromancer

            skillHandlers.Add(3331, new Dejion());
            skillHandlers.Add(2316, new SoulBurn());
            skillHandlers.Add(2317, new SpiritBurn());
            skillHandlers.Add(3288, new DarkLight());
            skillHandlers.Add(3330, new EvilSoul());
            skillHandlers.Add(3332, new ChaosGait());
            skillHandlers.Add(2318, new AbsorbHpWeapon());
            skillHandlers.Add(2320, new SummobLemures());
            skillHandlers.Add(961, new LemuresHpUp());
            skillHandlers.Add(963, new LemuresMatkUp());
            skillHandlers.Add(962, new LemuresAtkUp());
            skillHandlers.Add(2321, new HealLemures());
            skillHandlers.Add(2319, new Rebone());
            skillHandlers.Add(3121, new NeKuRoMaNShi());
            skillHandlers.Add(315, new ChgstDamUp());
            skillHandlers.Add(3122, new SkillDefinations.Necromancer.TrDrop2());
            skillHandlers.Add(3297, new Terror());
            skillHandlers.Add(3324, new SumDeath());
            skillHandlers.Add(3325, new SumDeath2());
            skillHandlers.Add(3326, new SumDeath3());
            skillHandlers.Add(3327, new SumDeath4());
            skillHandlers.Add(3328, new SumDeath5());
            skillHandlers.Add(3329, new SumDeath6());

            #endregion

            #region Soul

            skillHandlers.Add(4450, new Soul()); //SKILL_P_T_SWORDMAN,光戰士之魂
            skillHandlers.Add(4451, new Soul()); //SKILL_P_T_KINGHT,聖騎士之魂
            skillHandlers.Add(4452, new Soul()); //SKILL_P_T_ASSASSIN,暗殺者之魂
            skillHandlers.Add(4453, new Soul()); //SKILL_P_T_ARCHER,弓手之魂
            skillHandlers.Add(4454, new Soul()); //SKILL_P_T_MAGIC,魔導士之魂
            skillHandlers.Add(4455, new Soul()); //SKILL_P_T_ELEMENTAL,元素術師之魂
            skillHandlers.Add(4460, new Soul()); //SKILL_P_T_DRUID,神官之魂
            skillHandlers.Add(4461, new Soul()); //SKILL_P_T_KABBALIST,暗黑神官之魂
            skillHandlers.Add(4462, new Soul()); //SKILL_P_T_BLACKSMITH,鐵匠之魂
            skillHandlers.Add(4463, new Soul()); //SKILL_P_T_ALCHEMIST,鍊金術師之魂
            skillHandlers.Add(4464, new Soul()); //SKILL_P_T_EXPLORER,探險家之魂
            skillHandlers.Add(4465, new Soul()); //SKILL_P_T_TRADER,拜金使之魂
            skillHandlers.Add(4466, new Soul()); //SKILL_P_T_JOKER,道化師之魂

            #endregion

            #region DarkStalker

            skillHandlers.Add(2357, new DarkMist());
            skillHandlers.Add(2356, new LifeSteal());
            skillHandlers.Add(3289, new DegradetionDarkFlare());
            skillHandlers.Add(3290, new DegradetionDarkFlare());
            skillHandlers.Add(2403, new FlareSting());
            skillHandlers.Add(2404, new FlareSting2());
            skillHandlers.Add(2405, new BloodAbsrd());
            skillHandlers.Add(3120, new Spell());
            skillHandlers.Add(957, new NecroResu());
            skillHandlers.Add(314, new ChgstSwoDamUp());
            skillHandlers.Add(2277, new CancelLightCircle());
            skillHandlers.Add(958, new DarkProtect());
            skillHandlers.Add(935, new ElementAddUp(Elements.Dark, "DarkAddUp"));
            skillHandlers.Add(2278, new LightSeal());
            skillHandlers.Add(2279, new BradStigma());
            skillHandlers.Add(2358, new HpLostDamUp());
            skillHandlers.Add(979, new HpDownToDamUp());
            skillHandlers.Add(2406, new DarknessOfNight());
            skillHandlers.Add(2407, new DarknessOfNight2());
            skillHandlers.Add(500, new EleDamUp());
            skillHandlers.Add(501, new EleHitUp());
            skillHandlers.Add(502, new EleAvoUp());

            #endregion

            #region Striker

            skillHandlers.Add(2149, new ElementArrow(Elements.Fire));
            skillHandlers.Add(2150, new ElementArrow(Elements.Water));
            skillHandlers.Add(2151, new ElementArrow(Elements.Earth));
            skillHandlers.Add(2152, new ElementArrow(Elements.Wind));
            skillHandlers.Add(2220, new PotionArrow());
            skillHandlers.Add(2190, new LightDarkArrow(Elements.Holy));
            skillHandlers.Add(2191, new LightDarkArrow(Elements.Dark));
            skillHandlers.Add(313, new HuntingTactics());
            skillHandlers.Add(2267, new BowCastCancelOne());
            skillHandlers.Add(2266, new BlastArrow());
            skillHandlers.Add(310, new ChgstArrDamUp());
            skillHandlers.Add(2385, new ArmBreak());
            skillHandlers.Add(6500, new PetBirdAtkRowCircle());
            skillHandlers.Add(6501, new PetBirdAtkRowCircle2());
            skillHandlers.Add(6306, new DogHateUpCircle());
            skillHandlers.Add(6307, new PetDogHateUpCircle());
            skillHandlers.Add(6502, new BirdAtk());
            skillHandlers.Add(6503, new PetBirdAtk());
            skillHandlers.Add(6308, new PetCastSkill(6309, "ANIMAL"));
            skillHandlers.Add(6309, new PetDogAtkCircle());
            skillHandlers.Add(6550, new BirdDamUp());
            skillHandlers.Add(6350, new DogHpUp());
            skillHandlers.Add(6310, new PetCastSkill(6311, "ANIMAL"));
            skillHandlers.Add(6311, new PetDogDefUp());

            #endregion

            #region Gunner

            skillHandlers.Add(2285, new FastDraw());
            skillHandlers.Add(2286, new PluralityShot());
            skillHandlers.Add(2287, new ChargeShot());
            skillHandlers.Add(2289, new GrenadeShot());
            skillHandlers.Add(2291, new GrenadeSlow());
            skillHandlers.Add(2292, new GrenadeStan());
            skillHandlers.Add(2163, new StunShot());
            skillHandlers.Add(2288, new BurstShot());
            skillHandlers.Add(974, new CQB());
            skillHandlers.Add(2364, new GunCancel());
            skillHandlers.Add(2418, new VitalShot());
            skillHandlers.Add(2419, new ClothCrest());
            skillHandlers.Add(126, new RifleGunDamUp());
            skillHandlers.Add(2290, new ApiBullet());
            skillHandlers.Add(210, new GunHitUp());
            skillHandlers.Add(2293, new OverRange());
            skillHandlers.Add(2363, new BulletDance());
            skillHandlers.Add(2420, new PrecisionFire());
            skillHandlers.Add(2421, new CanisterShot());

            #endregion

            #region Explorer

            skillHandlers.Add(2222, new CaveHiding());
            skillHandlers.Add(2392, new Blinding());
            skillHandlers.Add(2221, new CaveBivouac());
            skillHandlers.Add(2212, new InsectAnalysis());
            skillHandlers.Add(2213, new BirdAnalysis());
            skillHandlers.Add(2215, new AnimalAnalysis());
            skillHandlers.Add(2264, new SupportInfo());
            skillHandlers.Add(953, new BaitTrap());
            skillHandlers.Add(311, new TrapDamUp());
            skillHandlers.Add(944, new BoostHp());
            skillHandlers.Add(2391, new FakeDeath());
            skillHandlers.Add(6300, new PetCastSkill(6301, "ANIMAL"));
            skillHandlers.Add(6301, new PetDogSlash());
            skillHandlers.Add(6302, new PetCastSkill(6303, "ANIMAL"));
            skillHandlers.Add(6303, new PetDogStan());
            skillHandlers.Add(6304, new PetCastSkill(6305, "ANIMAL"));
            skillHandlers.Add(6305, new PetDogLineatk());
            skillHandlers.Add(2171, new BarbedTrap());
            skillHandlers.Add(2172, new Bungestac());
            skillHandlers.Add(412, new InsDamUp());
            skillHandlers.Add(413, new InsHitUp());
            skillHandlers.Add(414, new InsAvoUp());
            skillHandlers.Add(3347, new AbsorbSpWeapon());
            skillHandlers.Add(2478, new FascinationBox()); //JOB50

            #endregion

            #region TreasureHunter

            skillHandlers.Add(2336, new BackRush());
            skillHandlers.Add(2337, new Catch());
            skillHandlers.Add(2340, new ConthWhip());
            skillHandlers.Add(2373, new WhipFlourish());
            skillHandlers.Add(2426, new Caution());
            skillHandlers.Add(2372, new Snatch());
            skillHandlers.Add(2427, new PullWhip());
            skillHandlers.Add(2430, new SonicWhip());
            skillHandlers.Add(2431, new SonicWhipSEQ());
            skillHandlers.Add(2432, new SonicWhipSEQ2());
            skillHandlers.Add(129, new RopeDamUp());
            skillHandlers.Add(2341, new WeaponRemove());
            skillHandlers.Add(2428, new Warn()); //警戒
            skillHandlers.Add(2342, new ArmorRemove());
            skillHandlers.Add(134, new UnlockDamUp());
            skillHandlers.Add(2429, new Escape());
            skillHandlers.Add(960, new GoodLucky()); //2018/4/5实装
            skillHandlers.Add(2339, new SearchTreasure()); //2018/4/9實裝

            #endregion

            #region Merchant

            skillHandlers.Add(702, new Packing());
            skillHandlers.Add(703, new BuyRateDown());
            skillHandlers.Add(704, new SellRateUp());
            skillHandlers.Add(2173, new AtkUpOne());
            skillHandlers.Add(2180, new SunSofbley());
            skillHandlers.Add(2186, new Magrow());

            #endregion

            #region Trader

            skillHandlers.Add(2394, new BugRand());
            skillHandlers.Add(705, new Trust());
            skillHandlers.Add(906, new BagDamUp());
            skillHandlers.Add(811, new HumanInfo());
            skillHandlers.Add(2223, new Shift());
            skillHandlers.Add(2225, new AgiDexUpOne());
            skillHandlers.Add(948, new BagCapDamup());
            skillHandlers.Add(2227, new Abetment());
            skillHandlers.Add(706, new Connection());
            skillHandlers.Add(2218, new HumanAnalysis());
            skillHandlers.Add(945, new BoostCritical());
            skillHandlers.Add(6400, new HumCustomary());
            skillHandlers.Add(6401, new PetHumCustomary());
            skillHandlers.Add(6404, new PetAtkupSelf());
            skillHandlers.Add(6405, new PetHitupSelf());
            skillHandlers.Add(6406, new PetDefupSelf());
            skillHandlers.Add(6402, new HumAdditional());
            skillHandlers.Add(6403, new PetHumAdditional());
            skillHandlers.Add(6407, new PetSlash());
            skillHandlers.Add(6408, new PetIai());
            skillHandlers.Add(6409, new PetProvocation());
            skillHandlers.Add(6410, new PetSennpuuken());
            skillHandlers.Add(6411, new PetMeditation());
            skillHandlers.Add(6450, new HumHealRateUp());

            #endregion

            #region Gambler

            skillHandlers.Add(3286, new RandHeal());
            skillHandlers.Add(3287, new RouletteHeal());
            skillHandlers.Add(2348, new AtkDownOne());
            skillHandlers.Add(2374, new CardBoomEran());
            skillHandlers.Add(2350, new RandDamOne());
            skillHandlers.Add(2377, new DoubleUp());
            skillHandlers.Add(2375, new CoinShot());
            skillHandlers.Add(972, new Blackleg());
            skillHandlers.Add(2347, new Slot());
            skillHandlers.Add(973, new BadLucky());
            skillHandlers.Add(2376, new SkillBreak());
            skillHandlers.Add(2436, new TrickDice());
            skillHandlers.Add(2433, new SumArcanaCard());
            //skillHandlers.Add(2432, new SkillDefinations.Gambler.SumArcanaCard2());
            //skillHandlers.Add(2431, new SkillDefinations.Gambler.SumArcanaCard3());
            skillHandlers.Add(2434, new SumArcanaCard4());
            skillHandlers.Add(2435, new SumArcanaCard5());
            skillHandlers.Add(2351, new RandChgstateCircle());
            skillHandlers.Add(2439, new FlowerCard());
            skillHandlers.Add(2440, new FlowerCardSEQ());
            skillHandlers.Add(2441, new FlowerCardSEQ2());

            #endregion

            #region Pet

            skillHandlers.Add(6424, new Revive(2));
            skillHandlers.Add(6425, new Revive(5));

            #endregion

            #region Breeder

            skillHandlers.Add(1000, new GrowUp());
            skillHandlers.Add(1001, new Biology());
            skillHandlers.Add(1002, new LionPower());
            skillHandlers.Add(1003, new Reins());
            skillHandlers.Add(2442, new TheTrust());
            skillHandlers.Add(2443, new Metamorphosis());
            skillHandlers.Add(2444, new PetDelayCancel());
            skillHandlers.Add(2445, new Akurobattoibeijon());
            skillHandlers.Add(2446, new HealFire());
            skillHandlers.Add(2447, new Encouragement());

            #endregion

            #region Gardener

            skillHandlers.Add(2453, new IAmTree());
            skillHandlers.Add(2449, new GardenerSkill());
            skillHandlers.Add(1006, new MoogCoalUp());
            skillHandlers.Add(2025, new Synthese());
            skillHandlers.Add(2455, new Cabin());
            skillHandlers.Add(1004, new GadenMaster());
            skillHandlers.Add(1005, new Topiary());
            skillHandlers.Add(1007, new Gardening());
            skillHandlers.Add(2452, new WeatherControl());
            skillHandlers.Add(2451, new HeavenlyControl());
            skillHandlers.Add(2454, new Gathering());

            #endregion

            #region 新Boss

            skillHandlers.Add(22000, new B1());
            skillHandlers.Add(22008, new WaterBall());

            #endregion

            #region 旅者

            skillHandlers.Add(23000, new SkillDefinations.Traveler.ChainLightning());
            skillHandlers.Add(23001, new HartHeal());
            skillHandlers.Add(23002, new ThunderFall());
            skillHandlers.Add(23003, new ThunderFall_Effect());
            skillHandlers.Add(23004, new Silva());
            skillHandlers.Add(23005, new SkillDefinations.Traveler.EarthQuake());
            skillHandlers.Add(23006, new EarthQuake_Effect());

            #endregion

            #region 武器技能

            skillHandlers.Add(24000, new WA_Neutral());
            skillHandlers.Add(1508, new MinCriRateUp());

            #endregion

            #region FL1

            skillHandlers.Add(100, new MaxHPUp());
            skillHandlers.Add(107, new SwordMastery());
            skillHandlers.Add(109, new SpearMastery());
            skillHandlers.Add(122, new TwoAxeMastery());
            skillHandlers.Add(116, new ShieldMastery());
            skillHandlers.Add(2112, new SkillDefinations.Swordman.ConfuseBlow());
            skillHandlers.Add(2138, new LightningSpear());
            skillHandlers.Add(2121, new ChargeStrike());

            #endregion

            #region FL2-1

            skillHandlers.Add(2354, new SkillDefinations.BountyHunter.Gravity());
            skillHandlers.Add(2124, new Sinkuha());
            skillHandlers.Add(119, new TwoHandMastery());
            skillHandlers.Add(2002, new ATKUp());
            skillHandlers.Add(2239, new Transporter());
            skillHandlers.Add(25010, new FireSlash());
            skillHandlers.Add(25011, new ArmorBreaker());

            #endregion

            #region FL2-2

            skillHandlers.Add(2228, new HolyBlade());
            skillHandlers.Add(2276, new DarkVacuum());
            skillHandlers.Add(123, new RapierMastery());
            skillHandlers.Add(3170, new SkillDefinations.Knight.Healing());
            skillHandlers.Add(120, new TwoSpearMastery());
            skillHandlers.Add(2383, new Appeal());
            skillHandlers.Add(2249, new StrikeSpear());
            skillHandlers.Add(25020, new IceStab());
            skillHandlers.Add(25021, new ShieldDefence());
            skillHandlers.Add(25022, new ShieldBash());

            #endregion

            #region FR1

            skillHandlers.Add(2042, new Hiding());
            skillHandlers.Add(102, new MaxSPUp());
            skillHandlers.Add(2000, new HitMeleeUp());
            skillHandlers.Add(2110, new SkillDefinations.Swordman.Blow());


            skillHandlers.Add(2035, new Synthese());

            skillHandlers.Add(2126, new VitalAttack());
            skillHandlers.Add(2119, new SkillDefinations.Scout.Brandish());
            skillHandlers.Add(114, new LAvoUp());
            skillHandlers.Add(2129, new ChargeArrow());
            skillHandlers.Add(2148, new PluralityArrow());

            #endregion

            #region FR2-1

            skillHandlers.Add(2113, new SkillDefinations.Swordman.StunBlow());
            skillHandlers.Add(2043, new Cloaking());
            skillHandlers.Add(2384, new Raid());
            skillHandlers.Add(977, new AvoidUp());
            skillHandlers.Add(2068, new BackAtk());
            skillHandlers.Add(312, new CriDamUp());
            skillHandlers.Add(118, new ClawMastery());
            skillHandlers.Add(2360, new CyclOne());
            skillHandlers.Add(2140, new PosionNeedle());
            skillHandlers.Add(26010, new ThrowThrowThrow());

            #endregion

            #region FR2-2

            skillHandlers.Add(951, new ShotStance());
            skillHandlers.Add(2049, new LHitUp());
            //skillHandlers.Add(2130, new SkillDefinations.Striker.BlastArrow());//贯通之箭,技能体指定错误
            skillHandlers.Add(2386, new ArrowGroove());
            skillHandlers.Add(2144, new SkillDefinations.FR2_2.FireArrow());
            skillHandlers.Add(2145, new SkillDefinations.FR2_2.WaterArrow());
            skillHandlers.Add(2146, new SkillDefinations.FR2_2.EarthArrow());
            skillHandlers.Add(2147, new SkillDefinations.FR2_2.WindArrow());
            skillHandlers.Add(2206, new DistanceArrow());
            skillHandlers.Add(26020, new SlowArrow());

            #endregion

            #region 自定义技能

            skillHandlers.Add(4901, new ModeChange());
            skillHandlers.Add(25000, new PassiveStr());
            skillHandlers.Add(25001, new PassiveAgi());
            skillHandlers.Add(25002, new PassiveInt());
            skillHandlers.Add(25003, new PassiveVit());
            skillHandlers.Add(25004, new PassiveDex());
            skillHandlers.Add(25005, new PassiveMag());
            skillHandlers.Add(27052, new HolyLight());
            skillHandlers.Add(27053, new KyrieEleison());
            skillHandlers.Add(27054, new HighHeal());
            skillHandlers.Add(27055, new Assumptio());
            skillHandlers.Add(27056, new Judex());
            skillHandlers.Add(27057, new Adoramus());
            skillHandlers.Add(27058, new Oratio());
            skillHandlers.Add(27059, new SkillDefinations.SunFlowerAdditions.TurnUndead());
            skillHandlers.Add(27060, new EquipChange());

            #endregion

            #region Partner

            skillHandlers.Add(15482, new TrialsInHeavenAndHell()); //路西法专用技能
            skillHandlers.Add(15330, new Thousand()); //伊邪那美专用技能
            skillHandlers.Add(15331, new Yukimori()); //伊邪那美专用技能
            skillHandlers.Add(6873, new ShinStormSword()); //蝙蝠阿鲁玛专用技能
            skillHandlers.Add(20206, new ComeOn()); //清姬专用技能
            skillHandlers.Add(15005, new MachineNurseAria()); //亚里亚专用技能
            skillHandlers.Add(15007, new Emission()); //亚里亚专用技能
            skillHandlers.Add(15010, new BlowAway()); //亚里亚专用技能
            skillHandlers.Add(6890, new Urayahachinototo()); //美琴专用技能
            skillHandlers.Add(6891, new DontCareAnymore()); //美琴专用技能
            skillHandlers.Add(20048, new DarkWorldWind()); //阿露卡多专用技能
            skillHandlers.Add(6676, new SkillDefinations.Global.DarkMist()); //阿露卡多专用技能
            skillHandlers.Add(6718, new Accept()); //阿露卡多专用技能
            skillHandlers.Add(6766, new NoFlashPlayer()); //罗蕾莱专用技能
            skillHandlers.Add(6765, new GoodByeRendezvous()); //罗蕾莱专用技能
            skillHandlers.Add(6758, new LeaveIt()); //玉藻专用技能
            skillHandlers.Add(6760, new TheRevelationOfKuo()); //玉藻专用技能
            skillHandlers.Add(15173, new LikeAPie()); //ECO猪专用技能
            skillHandlers.Add(15177, new HowDidYouDoIt()); //ECO猪专用技能
            skillHandlers.Add(15178, new YouLikeMe()); //ECO猪专用技能
            skillHandlers.Add(15179, new ImSoAngry()); //ECO猪专用技能
            skillHandlers.Add(15180, new EcoDay()); //ECO猪专用技能
            skillHandlers.Add(6918, new Kirieredison()); //雾绘专用技能
            skillHandlers.Add(6919, new PleaseTakeCareOfMe()); //雾绘专用技能
            skillHandlers.Add(6920, new TakeCareOfYou()); //雾绘专用技能
            skillHandlers.Add(6921, new ISupport()); //雾绘专用技能
            skillHandlers.Add(7127, new AllShot()); //咕咕鸡阿鲁玛专用技能
            skillHandlers.Add(15421, new MostThingsBurn()); //咕咕鸡阿鲁玛专用技能
            skillHandlers.Add(15422, new ContinuousShooting()); //咕咕鸡阿鲁玛专用技能
            skillHandlers.Add(9294, new BlowOfFriendship()); //装备咕咕鸡阿鲁玛后玩家可用技能
            skillHandlers.Add(7144, new Demolition()); //炸脖龙专用技能
            skillHandlers.Add(7145, new TimeDeath()); //炸脖龙专用技能
            skillHandlers.Add(7146, new BanderSnatch()); //炸脖龙专用技能
            skillHandlers.Add(15458, new SoBusy()); //炸脖龙专用技能
            skillHandlers.Add(15459, new YouCanPraiseMore()); //炸脖龙专用技能
            skillHandlers.Add(15518, new ThePowerOfLoveMayle()); //守护神之心专用技能

            #endregion
        }
    }
}
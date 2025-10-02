using SagaLib;

namespace SagaDB.Actor
{
    public class Buff
    {
        public BitMask[] Buffs { get; set; } = new BitMask[12]
        {
            new BitMask(), new BitMask(), new BitMask(), new BitMask(), new BitMask(), new BitMask(), new BitMask(),
            new BitMask(), new BitMask(), new BitMask(), new BitMask(), new BitMask()
        };

        public void Clear()
        {
            Buffs[0].Value = 0;
            Buffs[1].Value = 0;
            Buffs[2].Value = 0;
            Buffs[3].Value = 0;
            Buffs[4].Value = 0;
            Buffs[5].Value = 0;
            Buffs[6].Value = 0;
            Buffs[7].Value = 0;
            Buffs[8].Value = 0;
            Buffs[9].Value = 0;
            Buffs[10].Value = 0;
            Buffs[11].Value = 0;
        }

        //#region Buffs

        public bool state190
        {
            get => Buffs[6].Test(0x00000001);
            set => Buffs[6].SetValue(0x00000001, value);
        }

        /// <summary>
        ///     オーバーワーク
        /// </summary>
        public bool OverWork
        {
            get => Buffs[6].Test(0x00000002);
            set => Buffs[6].SetValue(0x00000002, value);
        }

        /// <summary>
        ///     ディレイキャンセル
        /// </summary>
        public bool DelayCancel3RD
        {
            get => Buffs[6].Test(0x00000004);
            set => Buffs[6].SetValue(0x00000004, value);
        }

        /// <summary>
        ///     赤くなる
        /// </summary>
        public bool TurningRed
        {
            get => Buffs[6].Test(0x00000008);
            set => Buffs[6].SetValue(0x00000008, value);
        }

        /// <summary>
        ///     フェニックス
        /// </summary>
        public bool Phoenix
        {
            get => Buffs[6].Test(0x00000010);
            set => Buffs[6].SetValue(0x00000010, value);
        }

        /// <summary>
        ///     スタミナテイク
        /// </summary>
        public bool StaminaTake
        {
            get => Buffs[6].Test(0x00000020);
            set => Buffs[6].SetValue(0x00000020, value);
        }


        //public bool 未知BUFFER
        //{
        //    get
        //    {
        //        return buffs[6].Test(0x00000040);
        //    }
        //    set
        //    {
        //        buffs[6].SetValue(0x00000040, value);
        //    }
        //}

        /// <summary>
        ///     マナの守護
        /// </summary>
        public bool ManaGuard
        {
            get => Buffs[6].Test(0x00000080);
            set => Buffs[6].SetValue(0x00000080, value);
        }

        /// <summary>
        ///     チャンプモンスターキラー状態
        /// </summary>
        public bool StateOfMonsterKillerChamp
        {
            get => Buffs[6].Test(0x00000100);
            set => Buffs[6].SetValue(0x00000100, value);
        }

        /// <summary>
        ///     竜眼開放
        /// </summary>
        public bool OpenDragonEye
        {
            get => Buffs[6].Test(0x00000200);
            set => Buffs[6].SetValue(0x00000200, value);
        }

        /// <summary>
        ///     温泉効果
        /// </summary>
        public bool EffectOfHotSpring
        {
            get => Buffs[6].Test(0x00000400);
            set => Buffs[6].SetValue(0x00000400, value);
        }

        /// <summary>
        ///     武器属性無効化
        /// </summary>
        public bool WeaponElementInvalid
        {
            get => Buffs[6].Test(0x00000800);
            set => Buffs[6].SetValue(0x00000800, value);
        }

        /// <summary>
        ///     防御属性無効化
        /// </summary>
        public bool GuardElementInvalid
        {
            get => Buffs[6].Test(0x00001000);
            set => Buffs[6].SetValue(0x00001000, value);
        }

        /// <summary>
        ///     ロケットブースター点火
        /// </summary>
        public bool RocketBoosterIgintion
        {
            get => Buffs[6].Test(0x00002000);
            set => Buffs[6].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     斧头达人
        /// </summary>
        public bool MasterOfAxe
        {
            get => Buffs[6].Test(0x00004000);
            set => Buffs[6].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     剑达人
        /// </summary>
        public bool MasterOfSword
        {
            get => Buffs[6].Test(0x00008000);
            set => Buffs[6].SetValue(0x00008000, value);
        }

        /// <summary>
        ///     矛达人
        /// </summary>
        public bool MasterOfSpear
        {
            get => Buffs[6].Test(0x00010000);
            set => Buffs[6].SetValue(0x00010000, value);
        }

        /// <summary>
        ///     枪达人
        /// </summary>
        public bool MasterOfGun
        {
            get => Buffs[6].Test(0x00020000);
            set => Buffs[6].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     三转主要技能效果强力增强
        /// </summary>
        public bool MainSkillPowerUp3RD
        {
            get => Buffs[6].Test(0x00040000);
            set => Buffs[6].SetValue(0x00040000, value);
        }

        /// <summary>
        ///     三转HP增强
        /// </summary>
        public bool MaxHPUp3RD
        {
            get => Buffs[6].Test(0x00080000);
            set => Buffs[6].SetValue(0x00080000, value);
        }

        /// <summary>
        ///     三转MP增强
        /// </summary>
        public bool MaxMPUp3RD
        {
            get => Buffs[6].Test(0x00100000);
            set => Buffs[6].SetValue(0x00100000, value);
        }

        /// <summary>
        ///     三转SP增强
        /// </summary>
        public bool MaxSPUp3RD
        {
            get => Buffs[6].Test(0x00200000);
            set => Buffs[6].SetValue(0x000200000, value);
        }

        /// <summary>
        ///     三转ATK增强
        /// </summary>
        public bool AtkUp3RD
        {
            get => Buffs[6].Test(0x00400000);
            set => Buffs[6].SetValue(0x000400000, value);
        }

        /// <summary>
        ///     三转MATK增强
        /// </summary>
        public bool MagicAtkUp3RD
        {
            get => Buffs[6].Test(0x00800000);
            set => Buffs[6].SetValue(0x00800000, value);
        }

        /// <summary>
        ///     三转DEF增强
        /// </summary>
        public bool DefUp3RD
        {
            get => Buffs[6].Test(0x01000000);
            set => Buffs[6].SetValue(0x01000000, value);
        }

        /// <summary>
        ///     三转MDEF增强
        /// </summary>
        public bool MagicDefUp3RD
        {
            get => Buffs[6].Test(0x02000000);
            set => Buffs[6].SetValue(0x02000000, value);
        }

        /// <summary>
        ///     三转Hit增强
        /// </summary>
        public bool HitUp3RD
        {
            get => Buffs[6].Test(0x04000000);
            set => Buffs[6].SetValue(0x04000000, value);
        }

        /// <summary>
        ///     三转AVOID增强
        /// </summary>
        public bool AvoidUp3RD
        {
            get => Buffs[6].Test(0x08000000);
            set => Buffs[6].SetValue(0x08000000, value);
        }

        /// <summary>
        ///     三转cspd增强
        /// </summary>
        public bool CastSpeedUp3RD
        {
            get => Buffs[6].Test(0x10000000);
            set => Buffs[6].SetValue(0x10000000, value);
        }

        /// <summary>
        ///     三转PAYL增强
        /// </summary>
        public bool PaylUp3RD
        {
            get => Buffs[6].Test(0x20000000);
            set => Buffs[6].SetValue(0x20000000, value);
        }

        /// <summary>
        ///     三转CAPA增强
        /// </summary>
        public bool CAPAUp3RD
        {
            get => Buffs[6].Test(0x40000000);
            set => Buffs[6].SetValue(0x40000000, value);
        }

        /*public bool Unknow18
        {
            get
            {
                return buffs[6].Test(0x80000000);
            }
            set
            {
                buffs[6].SetValue(0x80000000, value);
            }
        }*/

        //#endregion

        //#region Buffs

        /// <summary>
        ///     三转HP吸收
        /// </summary>
        public bool HPDrain3RD
        {
            get => Buffs[7].Test(0x00000001);
            set => Buffs[7].SetValue(0x00000001, value);
        }

        /// <summary>
        ///     三转MP吸收
        /// </summary>
        public bool MPDrain3RD
        {
            get => Buffs[7].Test(0x00000002);
            set => Buffs[7].SetValue(0x00000002, value);
        }

        /// <summary>
        ///     三转SP吸收
        /// </summary>
        public bool SPDrain3RD
        {
            get => Buffs[7].Test(0x00000004);
            set => Buffs[7].SetValue(0x00000004, value);
        }

        public bool 无1
        {
            get => Buffs[7].Test(0x00000008);
            set => Buffs[7].SetValue(0x00000008, value);
        }

        public bool 无2
        {
            get => Buffs[7].Test(0x00000010);
            set => Buffs[7].SetValue(0x00000010, value);
        }

        public bool 无3
        {
            get => Buffs[7].Test(0x00000020);
            set => Buffs[7].SetValue(0x00000020, value);
        }

        public bool 三转波动伤害固定
        {
            get => Buffs[7].Test(0x00000040);
            set => Buffs[7].SetValue(0x00000040, value);
        }

        public bool 三转枪连弹
        {
            get => Buffs[7].Test(0x00000080);
            set => Buffs[7].SetValue(0x00000080, value);
        }

        public bool KillingMark
        {
            get => Buffs[7].Test(0x00000100);
            set => Buffs[7].SetValue(0x00000100, value);
        }

        public bool 三转ATK与MATK互换
        {
            get => Buffs[7].Test(0x00000200);
            set => Buffs[7].SetValue(0x00000200, value);
        }

        public bool 三转元素身体属性赋予
        {
            get => Buffs[7].Test(0x00000400);
            set => Buffs[7].SetValue(0x00000400, value);
        }

        public bool 三转元素武器属性赋予
        {
            get => Buffs[7].Test(0x00000800);
            set => Buffs[7].SetValue(0x00000800, value);
        }

        public bool 三转2足ATKUP
        {
            get => Buffs[7].Test(0x00001000);
            set => Buffs[7].SetValue(0x00001000, value);
        }

        public bool RobotUnknowStateDown3RD
        {
            get => Buffs[7].Test(0x00002000);
            set => Buffs[7].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     FO物质拘束
        /// </summary>
        public bool WeaponFobbiden3RD
        {
            get => Buffs[7].Test(0x00004000);
            set => Buffs[7].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     三转受伤害提升伤害标记
        /// </summary>
        public bool GetDamageUpDamageMark3RD
        {
            get => Buffs[7].Test(0x00008000);
            set => Buffs[7].SetValue(0x00008000, value);
        }

        /// <summary>
        ///     三转伤害降低精神标记
        /// </summary>
        public bool DamageReduceSpriteMark3RD
        {
            get => Buffs[7].Test(0x00010000);
            set => Buffs[7].SetValue(0x00010000, value);
        }

        /// <summary>
        ///     三转J速
        /// </summary>
        public bool JSpeed3RD
        {
            get => Buffs[7].Test(0x00020000);
            set => Buffs[7].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     三转人血管
        /// </summary>
        public bool 三转人血管
        {
            get => Buffs[7].Test(0x00040000);
            set => Buffs[7].SetValue(0x00040000, value);
        }

        /// <summary>
        ///     三转荆棘刺
        /// </summary>
        public bool 三转荆棘刺
        {
            get => Buffs[7].Test(0x00080000);
            set => Buffs[7].SetValue(0x00080000, value);
        }

        /// <summary>
        ///     三转鬼人斩
        /// </summary>
        public bool DevilStance
        {
            get => Buffs[7].Test(0x00100000);
            set => Buffs[7].SetValue(0x00100000, value);
        }

        /// <summary>
        ///     三转宙斯盾イージス
        /// </summary>
        public bool Aegis3RD
        {
            get => Buffs[7].Test(0x00200000);
            set => Buffs[7].SetValue(0x00200000, value);
        }

        public bool 三转凭依者封印
        {
            get => Buffs[7].Test(0x00400000);
            set => Buffs[7].SetValue(0x00400000, value);
        }

        public bool 三转四属性赋予アンプリエレメント
        {
            get => Buffs[7].Test(0x00800000);
            set => Buffs[7].SetValue(0x00800000, value);
        }

        public bool 三转铁匠2足DEFUP
        {
            get => Buffs[7].Test(0x01000000);
            set => Buffs[7].SetValue(0x01000000, value);
        }

        public bool 三转机器人UNKNOWS
        {
            get => Buffs[7].Test(0x02000000);
            set => Buffs[7].SetValue(0x02000000, value);
        }

        public bool 三转禁言レストスキル
        {
            get => Buffs[7].Test(0x04000000);
            set => Buffs[7].SetValue(0x04000000, value);
        }

        public bool 三转指定对象被会心率UPクリティカルマーキング
        {
            get => Buffs[7].Test(0x08000000);
            set => Buffs[7].SetValue(0x08000000, value);
        }

        public bool 三转凭依保护ソウルプロテクト
        {
            get => Buffs[7].Test(0x10000000);
            set => Buffs[7].SetValue(0x10000000, value);
        }

        public bool 三转見切り
        {
            get => Buffs[7].Test(0x20000000);
            set => Buffs[7].SetValue(0x20000000, value);
        }

        public bool 三转魔法抗体
        {
            get => Buffs[7].Test(0x40000000);
            set => Buffs[7].SetValue(0x40000000, value);
        }

        //#endregion

        //#region Buffs

        public bool 三转机器人攻速上升
        {
            get => Buffs[8].Test(0x00000001);
            set => Buffs[8].SetValue(0x00000001, value);
        }

        public bool 三转机器人攻速下降
        {
            get => Buffs[8].Test(0x00000002);
            set => Buffs[8].SetValue(0x00000002, value);
        }

        public bool 三转ウィークネスショット
        {
            get => Buffs[8].Test(0x00000004);
            set => Buffs[8].SetValue(0x00000004, value);
        }

        public bool 点火紫火
        {
            get => Buffs[8].Test(0x00000008);
            set => Buffs[8].SetValue(0x00000008, value);
        }

        public bool Unknow27
        {
            get => Buffs[8].Test(0x00000010);
            set => Buffs[8].SetValue(0x00000010, value);
        }

        public bool 三转红锤子ウェポンエンハンス

        {
            get => Buffs[8].Test(0x00000020);
            set => Buffs[8].SetValue(0x00000020, value);
        }

        public bool 三转レトリック
        {
            get => Buffs[8].Test(0x00000040);
            set => Buffs[8].SetValue(0x00000040, value);
        }

        public bool 师匠的加护
        {
            get => Buffs[8].Test(0x00000080);
            set => Buffs[8].SetValue(0x00000080, value);
        }

        public bool 三转モンスターチェンジ
        {
            get => Buffs[8].Test(0x00000100);
            set => Buffs[8].SetValue(0x00000100, value);
        }

        public bool 三转知识的书
        {
            get => Buffs[8].Test(0x00000200);
            set => Buffs[8].SetValue(0x00000200, value);
        }

        public bool 三转植物寄生
        {
            get => Buffs[8].Test(0x00000400);
            set => Buffs[8].SetValue(0x00000400, value);
        }

        public bool 三转パクティオ
        {
            get => Buffs[8].Test(0x00000800);
            set => Buffs[8].SetValue(0x00000800, value);
        }

        public bool 三转アドバンスアビリテイー
        {
            get => Buffs[8].Test(0x00001000);
            set => Buffs[8].SetValue(0x00001000, value);
        }

        public bool 三转フエンリル
        {
            get => Buffs[8].Test(0x00002000);
            set => Buffs[8].SetValue(0x00002000, value);
        }

        public bool Blocking
        {
            get => Buffs[8].Test(0x00004000);
            set => Buffs[8].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     FishingState
        /// </summary>
        public bool FishingState
        {
            get => Buffs[8].Test(0x00008000);
            set => Buffs[8].SetValue(0x00008000, value);
        }

        public bool 三转せーチウィークポイント
        {
            get => Buffs[8].Test(0x00010000);
            set => Buffs[8].SetValue(0x00010000, value);
        }

        public bool Unknow4
        {
            get => Buffs[8].Test(0x00020000);
            set => Buffs[8].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     冒险3转JOB50技能BUFF
        /// </summary>
        public bool アートフルトラップ
        {
            get => Buffs[8].Test(0x00040000);
            set => Buffs[8].SetValue(0x00040000, value);
        }

        public bool Unknow6
        {
            get => Buffs[8].Test(0x00080000);
            set => Buffs[8].SetValue(0x00080000, value);
        }

        public bool Unknow7
        {
            get => Buffs[8].Test(0x00100000);
            set => Buffs[8].SetValue(0x00100000, value);
        }

        public bool Unknow8
        {
            get => Buffs[8].Test(0x00200000);
            set => Buffs[8].SetValue(0x00200000, value);
        }

        public bool Unknow9
        {
            get => Buffs[8].Test(0x00400000);
            set => Buffs[8].SetValue(0x00400000, value);
        }

        /// <summary>
        ///     流水攻势
        /// </summary>
        public bool SwordEaseSp
        {
            get => Buffs[8].Test(0x00800000);
            set => Buffs[8].SetValue(0x00800000, value);
        }

        public bool Unknow11
        {
            get => Buffs[8].Test(0x01000000);
            set => Buffs[8].SetValue(0x01000000, value);
        }

        public bool Unknow12
        {
            get => Buffs[8].Test(0x02000000);
            set => Buffs[8].SetValue(0x02000000, value);
        }

        public bool Unknow13
        {
            get => Buffs[8].Test(0x04000000);
            set => Buffs[8].SetValue(0x04000000, value);
        }

        public bool Unknow14
        {
            get => Buffs[8].Test(0x08000000);
            set => Buffs[8].SetValue(0x08000000, value);
        }

        public bool Unknow15
        {
            get => Buffs[8].Test(0x10000000);
            set => Buffs[8].SetValue(0x10000000, value);
        }

        /// <summary>
        ///     2转MP吸收
        /// </summary>
        public bool Unknow16
        {
            get => Buffs[8].Test(0x20000000);
            set => Buffs[8].SetValue(0x20000000, value);
        }

        public bool Unknow17
        {
            get => Buffs[8].Test(0x40000000);
            set => Buffs[8].SetValue(0x40000000, value);
        }

        //#endregion

        //#region Buffs

        /// <summary>
        ///     Zombie
        /// </summary>
        public bool Zombie
        {
            get => Buffs[5].Test(0x00000001);
            set => Buffs[5].SetValue(0x00000001, value);
        }

        /// <summary>
        ///     リボーン
        /// </summary>
        public bool Reborn
        {
            get => Buffs[5].Test(0x00000002);
            set => Buffs[5].SetValue(0x00000002, value);
        }

        /// <summary>
        ///     演奏中
        /// </summary>
        public bool Playing
        {
            get => Buffs[5].Test(0x00000004);
            set => Buffs[5].SetValue(0x00000004, value);
        }

        /// <summary>
        /// </summary>
        public bool 羽交い絞め
        {
            get => Buffs[5].Test(0x00000008);
            set => Buffs[5].SetValue(0x00000008, value);
        }

        /// <summary>
        ///     光魔法封印
        /// </summary>
        public bool LightMagicSealed
        {
            get => Buffs[5].Test(0x00000010);
            set => Buffs[5].SetValue(0x00000010, value);
        }

        /// <summary>
        ///     オーバーレンジ
        /// </summary>
        public bool OverRange
        {
            get => Buffs[5].Test(0x00000020);
            set => Buffs[5].SetValue(0x00000020, value);
        }

        /// <summary>
        ///     2转吸血
        /// </summary>
        public bool LifeTake
        {
            get => Buffs[5].Test(0x00000040);
            set => Buffs[5].SetValue(0x00000040, value);
        }

        /// <summary>
        ///     恐怖
        /// </summary>
        public bool Horrible
        {
            get => Buffs[5].Test(0x00000080);
            set => Buffs[5].SetValue(0x00000080, value);
        }

        /// <summary>
        ///     経験値上昇
        /// </summary>
        public bool EXPUp
        {
            get => Buffs[5].Test(0x00000100);
            set => Buffs[5].SetValue(0x00000100, value);
        }

        /// <summary>
        ///     パッシング
        /// </summary>
        public bool Passing
        {
            get => Buffs[5].Test(0x00000200);
            set => Buffs[5].SetValue(0x00000200, value);
        }

        /// <summary>
        ///     回復不可能
        /// </summary>
        public bool NoRegen
        {
            get => Buffs[5].Test(0x00000400);
            set => Buffs[5].SetValue(0x00000400, value);
        }

        /// <summary>
        ///     エンチャントブロック
        /// </summary>
        public bool EnchantmentBlock
        {
            get => Buffs[5].Test(0x00000800);
            set => Buffs[5].SetValue(0x00000800, value);
        }

        /// <summary>
        ///     ソリッドボディ
        /// </summary>
        public bool SolidBody
        {
            get => Buffs[5].Test(0x00001000);
            set => Buffs[5].SetValue(0x00001000, value);
        }

        /// <summary>
        ///     ブラッディウエポン
        /// </summary>
        public bool BloodyWeapon
        {
            get => Buffs[5].Test(0x00002000);
            set => Buffs[5].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     フレア
        /// </summary>
        public bool Flare
        {
            get => Buffs[5].Test(0x00004000);
            set => Buffs[5].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     ガンディレイキャンセル
        /// </summary>
        public bool GunDelayCancel
        {
            get => Buffs[5].Test(0x00008000);
            set => Buffs[5].SetValue(0x00008000, value);
        }

        /// <summary>
        ///     ダブルアップ
        /// </summary>
        public bool DoubleUp
        {
            get => Buffs[5].Test(0x00010000);
            set => Buffs[5].SetValue(0x00010000, value);
        }

        /// <summary>
        ///     ATフィールド
        /// </summary>
        public bool ATField
        {
            get => Buffs[5].Test(0x00020000);
            set => Buffs[5].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     根性
        /// </summary>
        public bool Spirit3RD
        {
            get => Buffs[5].Test(0x00040000);
            set => Buffs[5].SetValue(0x00040000, value);
        }

        /// <summary>
        ///     物理攻撃付加
        /// </summary>
        public bool PhysicAtkAddition
        {
            get => Buffs[5].Test(0x00080000);
            set => Buffs[5].SetValue(0x00080000, value);
        }

        /// <summary>
        ///     死んだふり
        /// </summary>
        public bool PlayingDead
        {
            get => Buffs[5].Test(0x00100000);
            set => Buffs[5].SetValue(0x00100000, value);
        }

        /// <summary>
        ///     パパ点火
        /// </summary>
        public bool PapaIgintion
        {
            get => Buffs[5].Test(0x00200000);
            set => Buffs[5].SetValue(0x00200000, value);
        }

        /// <summary>
        ///     TurningPurple
        /// </summary>
        public bool TurningPurple
        {
            get => Buffs[5].Test(0x00400000);
            set => Buffs[5].SetValue(0x00400000, value);
        }

        /// <summary>
        ///     精密射撃
        /// </summary>
        public bool PrecisionFire
        {
            get => Buffs[5].Test(0x00800000);
            set => Buffs[5].SetValue(0x00800000, value);
        }

        /// <summary>
        ///     オーバーチューン
        /// </summary>
        public bool OverTune
        {
            get => Buffs[5].Test(0x01000000);
            set => Buffs[5].SetValue(0x01000000, value);
        }

        /// <summary>
        ///     警戒
        /// </summary>
        public bool Warning
        {
            get => Buffs[5].Test(0x02000000);
            set => Buffs[5].SetValue(0x02000000, value);
        }

        /// <summary>
        ///     リフレクション
        /// </summary>
        public bool Reflection
        {
            get => Buffs[5].Test(0x04000000);
            set => Buffs[5].SetValue(0x04000000, value);
        }

        /// <summary>
        ///     エンチャントウエポン
        /// </summary>
        public bool EnchantWeapon
        {
            get => Buffs[5].Test(0x08000000);
            set => Buffs[5].SetValue(0x08000000, value);
        }

        /// <summary>
        ///     邪恶灵魂
        /// </summary>
        public bool Oritorio
        {
            get => Buffs[5].Test(0x10000000);
            set => Buffs[5].SetValue(0x10000000, value);
        }

        public bool イビルソウル
        {
            get => Buffs[5].Test(0x20000000);
            set => Buffs[5].SetValue(0x20000000, value);
        }

        /// <summary>
        ///     フレイムハート
        /// </summary>
        public bool FlameHart
        {
            get => Buffs[5].Test(0x40000000);
            set => Buffs[5].SetValue(0x40000000, value);
        }

        /*public bool アトラクトマーチ
        {
            get
            {
                return buffs[5].Test(0x80000000);
            }
            set
            {
                buffs[5].SetValue(0x80000000, value);
            }
        }*/

        //#endregion

        //#region Buffs

        /// <summary>
        ///     最大HP減少
        /// </summary>
        public bool MaxHPDown
        {
            get => Buffs[4].Test(0x00000001);
            set => Buffs[4].SetValue(0x00000001, value);
        }

        /// <summary>
        ///     最大MP減少
        /// </summary>
        public bool MaxMPDown
        {
            get => Buffs[4].Test(0x00000002);
            set => Buffs[4].SetValue(0x00000002, value);
        }

        /// <summary>
        ///     最大SP減少
        /// </summary>
        public bool MaxSPDown
        {
            get => Buffs[4].Test(0x00000004);
            set => Buffs[4].SetValue(0x00000004, value);
        }

        /// <summary>
        ///     最小攻撃力減少
        /// </summary>
        public bool MinAtkDown
        {
            get => Buffs[4].Test(0x00000010);
            set => Buffs[4].SetValue(0x00000010, value);
        }

        /// <summary>
        ///     最大攻撃力減少
        /// </summary>
        public bool MaxAtkDown
        {
            get => Buffs[4].Test(0x00000020);
            set => Buffs[4].SetValue(0x00000020, value);
        }

        /// <summary>
        ///     最小魔法攻撃力減少
        /// </summary>
        public bool MinMagicAtkDown
        {
            get => Buffs[4].Test(0x00000040);
            set => Buffs[4].SetValue(0x00000040, value);
        }

        /// <summary>
        ///     最大魔法攻撃力減少
        /// </summary>
        public bool MaxMagicAtkDown
        {
            get => Buffs[4].Test(0x00000080);
            set => Buffs[4].SetValue(0x00000080, value);
        }

        /// <summary>
        ///     防御率減少
        /// </summary>
        public bool DefRateDown
        {
            get => Buffs[4].Test(0x00000100);
            set => Buffs[4].SetValue(0x00000100, value);
        }

        /// <summary>
        ///     防御力減少
        /// </summary>
        public bool DefDown
        {
            get => Buffs[4].Test(0x00000200);
            set => Buffs[4].SetValue(0x00000200, value);
        }

        /// <summary>
        ///     魔法防御率減少
        /// </summary>
        public bool MagicDefRateDown
        {
            get => Buffs[4].Test(0x00000400);
            set => Buffs[4].SetValue(0x00000400, value);
        }

        /// <summary>
        ///     魔法防御力減少
        /// </summary>
        public bool MagicDefDown
        {
            get => Buffs[4].Test(0x00000800);
            set => Buffs[4].SetValue(0x00000800, value);
        }

        /// <summary>
        ///     近距離命中率減少
        /// </summary>
        public bool ShortHitDown
        {
            get => Buffs[4].Test(0x00001000);
            set => Buffs[4].SetValue(0x00001000, value);
        }

        /// <summary>
        ///     遠距離命中率減少
        /// </summary>
        public bool LongHitDown
        {
            get => Buffs[4].Test(0x00002000);
            set => Buffs[4].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     魔法命中率減少
        /// </summary>
        public bool MagicHitDown
        {
            get => Buffs[4].Test(0x00004000);
            set => Buffs[4].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     近距離回避率減少
        /// </summary>
        public bool ShortDodgeDown
        {
            get => Buffs[4].Test(0x00008000);
            set => Buffs[4].SetValue(0x00008000, value);
        }

        /// <summary>
        ///     遠距離回避率減少
        /// </summary>
        public bool LongDodgeDown
        {
            get => Buffs[4].Test(0x00010000);
            set => Buffs[4].SetValue(0x00010000, value);
        }

        /// <summary>
        ///     魔法抵抗率減少
        /// </summary>
        public bool MagicAvoidDown
        {
            get => Buffs[4].Test(0x00020000);
            set => Buffs[4].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     クリティカル率減少
        /// </summary>
        public bool CriticalRateDown

        {
            get => Buffs[4].Test(0x00040000);
            set => Buffs[4].SetValue(0x00040000, value);
        }

        /// <summary>
        ///     クリティカル回避率減少
        /// </summary>
        public bool CriticalDodgeDown
        {
            get => Buffs[4].Test(0x00080000);
            set => Buffs[4].SetValue(0x00080000, value);
        }

        /// <summary>
        ///     HP回復率減少
        /// </summary>
        public bool HPRegenDown
        {
            get => Buffs[4].Test(0x00100000);
            set => Buffs[4].SetValue(0x00100000, value);
        }

        /// <summary>
        ///     MP回復率減少
        /// </summary>
        public bool MPRegenDown
        {
            get => Buffs[4].Test(0x00200000);
            set => Buffs[4].SetValue(0x00200000, value);
        }

        /// <summary>
        ///     SP回復率減少
        /// </summary>
        public bool SPRegenDown
        {
            get => Buffs[4].Test(0x00400000);
            set => Buffs[4].SetValue(0x00400000, value);
        }

        /// <summary>
        ///     攻撃スピード減少
        /// </summary>
        public bool AttackSpeedDown
        {
            get => Buffs[4].Test(0x00800000);
            set => Buffs[4].SetValue(0x00800000, value);
        }

        /// <summary>
        ///     詠唱スピード減少
        /// </summary>
        public bool CastSpeedDown
        {
            get => Buffs[4].Test(0x01000000);
            set => Buffs[4].SetValue(0x01000000, value);
        }

        /// <summary>
        ///     STR減少
        /// </summary>
        public bool STRDown
        {
            get => Buffs[4].Test(0x02000000);
            set => Buffs[4].SetValue(0x02000000, value);
        }

        /// <summary>
        ///     DEX減少
        /// </summary>
        public bool DEXDown
        {
            get => Buffs[4].Test(0x04000000);
            set => Buffs[4].SetValue(0x04000000, value);
        }

        /// <summary>
        ///     INT減少
        /// </summary>
        public bool INTDown
        {
            get => Buffs[4].Test(0x08000000);
            set => Buffs[4].SetValue(0x08000000, value);
        }

        /// <summary>
        ///     VIT減少
        /// </summary>
        public bool VITDown
        {
            get => Buffs[4].Test(0x10000000);
            set => Buffs[4].SetValue(0x10000000, value);
        }

        /// <summary>
        ///     AGI減少
        /// </summary>
        public bool AGIDown
        {
            get => Buffs[4].Test(0x20000000);
            set => Buffs[4].SetValue(0x20000000, value);
        }

        /// <summary>
        ///     MAG減少
        /// </summary>
        public bool MAGDown
        {
            get => Buffs[4].Test(0x40000000);
            set => Buffs[4].SetValue(0x40000000, value);
        }

        //#endregion

        //#region Buffs

        /// <summary>
        ///     武器の無属性上昇
        /// </summary>
        public bool WeaponNatureElementUp
        {
            get => Buffs[2].Test(0x1);
            set => Buffs[2].SetValue(0x1, value);
        }

        /// <summary>
        ///     武器の火属性上昇
        /// </summary>
        public bool WeaponFireElementUp
        {
            get => Buffs[2].Test(0x2);
            set => Buffs[2].SetValue(0x2, value);
        }

        /// <summary>
        ///     武器の水属性上昇
        /// </summary>
        public bool WeaponWaterElementUp
        {
            get => Buffs[2].Test(0x4);
            set => Buffs[2].SetValue(0x4, value);
        }

        /// <summary>
        ///     武器の水属性上昇
        /// </summary>
        public bool WeaponWindElementUp
        {
            get => Buffs[2].Test(0x8);
            set => Buffs[2].SetValue(0x8, value);
        }

        /// <summary>
        ///     武器の土属性上昇
        /// </summary>
        public bool WeaponEarthElementUp
        {
            get => Buffs[2].Test(0x10);
            set => Buffs[2].SetValue(0x10, value);
        }

        /// <summary>
        ///     武器の光属性上昇
        /// </summary>
        public bool WeaponHolyElementUp
        {
            get => Buffs[2].Test(0x20);
            set => Buffs[2].SetValue(0x20, value);
        }

        /// <summary>
        ///     武器の闇属性上昇
        /// </summary>
        public bool WeaponDarkElementUp
        {
            get => Buffs[2].Test(0x40);
            set => Buffs[2].SetValue(0x40, value);
        }

        /// <summary>
        ///     武器の無属性減少
        /// </summary>
        public bool WeaponNatureElementDown
        {
            get => Buffs[2].Test(0x00000080);
            set => Buffs[2].SetValue(0x00000080, value);
        }

        /// <summary>
        ///     武器の火属性減少
        /// </summary>
        public bool WeaponFireElementDown
        {
            get => Buffs[2].Test(0x00000100);
            set => Buffs[2].SetValue(0x00000100, value);
        }

        /// <summary>
        ///     武器の水属性減少
        /// </summary>
        public bool WeaponWaterElementDown
        {
            get => Buffs[2].Test(0x00000200);
            set => Buffs[2].SetValue(0x00000200, value);
        }

        /// <summary>
        ///     武器の風属性減少
        /// </summary>
        public bool WeaponWindElementDown
        {
            get => Buffs[2].Test(0x00000400);
            set => Buffs[2].SetValue(0x00000400, value);
        }

        /// <summary>
        ///     武器の土属性減少
        /// </summary>
        public bool WeaponEarthElementDown
        {
            get => Buffs[2].Test(0x00000800);
            set => Buffs[2].SetValue(0x00000800, value);
        }

        /// <summary>
        ///     武器の光属性減少
        /// </summary>
        public bool WeaponLightElementDown
        {
            get => Buffs[2].Test(0x00001000);
            set => Buffs[2].SetValue(0x00001000, value);
        }

        /// <summary>
        ///     武器の闇属性減少
        /// </summary>
        public bool WeaponDarkElementDown
        {
            get => Buffs[2].Test(0x00002000);
            set => Buffs[2].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     体の無属性上昇
        /// </summary>
        public bool BodyNatureElementUp
        {
            get => Buffs[2].Test(0x4000);
            set => Buffs[2].SetValue(0x4000, value);
        }

        /// <summary>
        ///     体の火属性上昇
        /// </summary>
        public bool BodyFireElementUp
        {
            get => Buffs[2].Test(0x8000);
            set => Buffs[2].SetValue(0x8000, value);
        }

        /// <summary>
        ///     体の水属性上昇
        /// </summary>
        public bool BodyWaterElementUp
        {
            get => Buffs[2].Test(0x10000);
            set => Buffs[2].SetValue(0x10000, value);
        }

        /// <summary>
        ///     体の風属性上昇
        /// </summary>
        public bool BodyWindElementUp
        {
            get => Buffs[2].Test(0x20000);
            set => Buffs[2].SetValue(0x20000, value);
        }

        /// <summary>
        ///     体の土属性上昇
        /// </summary>
        public bool BodyEarthElementUp
        {
            get => Buffs[2].Test(0x40000);
            set => Buffs[2].SetValue(0x40000, value);
        }

        /// <summary>
        ///     体の光属性上昇
        /// </summary>
        public bool BodyHolyElementUp
        {
            get => Buffs[2].Test(0x80000);
            set => Buffs[2].SetValue(0x80000, value);
        }

        /// <summary>
        ///     体の闇属性上昇
        /// </summary>
        public bool BodyDarkElementUp
        {
            get => Buffs[2].Test(0x100000);
            set => Buffs[2].SetValue(0x100000, value);
        }

        /// <summary>
        ///     体の無属性減少
        /// </summary>
        public bool BodyNatureElementDown
        {
            get => Buffs[2].Test(0x00200000);
            set => Buffs[2].SetValue(0x00200000, value);
        }

        /// <summary>
        ///     体の火属性減少
        /// </summary>
        public bool BodyFireElementDown
        {
            get => Buffs[2].Test(0x00400000);
            set => Buffs[2].SetValue(0x00400000, value);
        }

        /// <summary>
        ///     体の火属性減少
        /// </summary>
        public bool BodyWaterElementDown
        {
            get => Buffs[2].Test(0x100000);
            set => Buffs[2].SetValue(0x100000, value);
        }

        /// <summary>
        ///     体の風属性減少
        /// </summary>
        public bool BodyWindElementDown
        {
            get => Buffs[2].Test(0x01000000);
            set => Buffs[2].SetValue(0x01000000, value);
        }

        /// <summary>
        ///     体の土属性減少
        /// </summary>
        public bool BodyEarthElementDown
        {
            get => Buffs[2].Test(0x02000000);
            set => Buffs[2].SetValue(0x02000000, value);
        }

        /// <summary>
        ///     体の光属性減少
        /// </summary>
        public bool BodyLightElementDown
        {
            get => Buffs[2].Test(0x04000000);
            set => Buffs[2].SetValue(0x04000000, value);
        }

        /// <summary>
        ///     体の闇属性減少
        /// </summary>
        public bool BodyDarkElementDown
        {
            get => Buffs[2].Test(0x08000000);
            set => Buffs[2].SetValue(0x08000000, value);
        }

        //#endregion

        //#region Buffs

        /// <summary>
        ///     最大HP上昇
        /// </summary>
        public bool MaxHPUp
        {
            get => Buffs[3].Test(0x00000001);
            set => Buffs[3].SetValue(0x00000001, value);
        }

        /// <summary>
        ///     最大MP上昇
        /// </summary>
        public bool MaxMPUp
        {
            get => Buffs[3].Test(0x00000002);
            set => Buffs[3].SetValue(0x00000002, value);
        }

        /// <summary>
        ///     最大SP上昇
        /// </summary>
        public bool MaxSPUp
        {
            get => Buffs[3].Test(0x00000004);
            set => Buffs[3].SetValue(0x00000004, value);
        }

        /// <summary>
        ///     移動力上昇
        /// </summary>
        public bool MoveSpeedUp
        {
            get => Buffs[3].Test(0x00000008);
            set => Buffs[3].SetValue(0x00000008, value);
        }

        /// <summary>
        ///     最小攻撃力上昇
        /// </summary>
        public bool MinAtkUp
        {
            get => Buffs[3].Test(0x00000010);
            set => Buffs[3].SetValue(0x00000010, value);
        }

        /// <summary>
        ///     最大攻撃力上昇
        /// </summary>
        public bool MaxAtkUp
        {
            get => Buffs[3].Test(0x00000020);
            set => Buffs[3].SetValue(0x00000020, value);
        }

        /// <summary>
        ///     最小魔法攻撃力上昇
        /// </summary>
        public bool MinMagicAtkUp
        {
            get => Buffs[3].Test(0x00000040);
            set => Buffs[3].SetValue(0x00000040, value);
        }

        /// <summary>
        ///     最大魔法攻撃力上昇
        /// </summary>
        public bool MaxMagicAtkUp
        {
            get => Buffs[3].Test(0x00000080);
            set => Buffs[3].SetValue(0x00000080, value);
        }

        /// <summary>
        ///     防御率上昇
        /// </summary>
        public bool DefRateUp
        {
            get => Buffs[3].Test(0x00000100);
            set => Buffs[3].SetValue(0x00000100, value);
        }

        /// <summary>
        ///     防御力上昇
        /// </summary>
        public bool DefUp
        {
            get => Buffs[3].Test(0x00000200);
            set => Buffs[3].SetValue(0x00000200, value);
        }

        /// <summary>
        ///     魔法防御率上昇
        /// </summary>
        public bool MagicDefRateUp
        {
            get => Buffs[3].Test(0x00000400);
            set => Buffs[3].SetValue(0x00000400, value);
        }

        /// <summary>
        ///     魔法防御力上昇
        /// </summary>
        public bool MagicDefUp
        {
            get => Buffs[3].Test(0x00000800);
            set => Buffs[3].SetValue(0x00000800, value);
        }

        /// <summary>
        ///     近距離命中率上昇
        /// </summary>
        public bool ShortHitUp
        {
            get => Buffs[3].Test(0x00001000);
            set => Buffs[3].SetValue(0x00001000, value);
        }

        /// <summary>
        ///     遠距離命中率上昇
        /// </summary>
        public bool LongHitUp
        {
            get => Buffs[3].Test(0x00002000);
            set => Buffs[3].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     魔法命中率上昇
        /// </summary>
        public bool MagicHitUp
        {
            get => Buffs[3].Test(0x00004000);
            set => Buffs[3].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     近距離回避率上昇
        /// </summary>
        public bool ShortDodgeUp
        {
            get => Buffs[3].Test(0x00008000);
            set => Buffs[3].SetValue(0x00008000, value);
        }

        /// <summary>
        ///     遠距離回避上昇
        /// </summary>
        public bool LongDodgeUp
        {
            get => Buffs[3].Test(0x00010000);
            set => Buffs[3].SetValue(0x00010000, value);
        }

        /// <summary>
        ///     魔法抵抗上昇
        /// </summary>
        public bool MagicAvoidUp
        {
            get => Buffs[3].Test(0x00020000);
            set => Buffs[3].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     クリティカル率上昇
        /// </summary>
        public bool CriticalRateUp
        {
            get => Buffs[3].Test(0x00040000);
            set => Buffs[3].SetValue(0x00040000, value);
        }

        /// <summary>
        ///     クリティカル回避率上昇
        /// </summary>
        public bool CriticalDodgeUp
        {
            get => Buffs[3].Test(0x00080000);
            set => Buffs[3].SetValue(0x00080000, value);
        }

        /// <summary>
        ///     HP回復率上昇
        /// </summary>
        public bool HPRegenUp
        {
            get => Buffs[3].Test(0x00100000);
            set => Buffs[3].SetValue(0x00100000, value);
        }

        /// <summary>
        ///     MP回復率上昇
        /// </summary>
        public bool MPRegenUp
        {
            get => Buffs[3].Test(0x00200000);
            set => Buffs[3].SetValue(0x00200000, value);
        }

        /// <summary>
        ///     SP回復率上昇
        /// </summary>
        public bool SPRegenUp
        {
            get => Buffs[3].Test(0x00400000);
            set => Buffs[3].SetValue(0x00400000, value);
        }

        /// <summary>
        ///     攻撃スピード上昇
        /// </summary>
        public bool AttackSpeedUp
        {
            get => Buffs[3].Test(0x00800000);
            set => Buffs[3].SetValue(0x00800000, value);
        }

        /// <summary>
        ///     詠唱スピード上昇
        /// </summary>
        public bool CastSpeedUp
        {
            get => Buffs[3].Test(0x01000000);
            set => Buffs[3].SetValue(0x01000000, value);
        }

        /// <summary>
        ///     STR上昇
        /// </summary>
        public bool STRUp
        {
            get => Buffs[3].Test(0x02000000);
            set => Buffs[3].SetValue(0x02000000, value);
        }

        /// <summary>
        ///     DEX上昇
        /// </summary>
        public bool DEXUp
        {
            get => Buffs[3].Test(0x04000000);
            set => Buffs[3].SetValue(0x04000000, value);
        }

        /// <summary>
        ///     INT上昇
        /// </summary>
        public bool INTUp
        {
            get => Buffs[3].Test(0x08000000);
            set => Buffs[3].SetValue(0x08000000, value);
        }

        /// <summary>
        ///     VIT上昇
        /// </summary>
        public bool VITUp
        {
            get => Buffs[3].Test(0x10000000);
            set => Buffs[3].SetValue(0x10000000, value);
        }

        /// <summary>
        ///     AGI上昇
        /// </summary>
        public bool AGIUp
        {
            get => Buffs[3].Test(0x20000000);
            set => Buffs[3].SetValue(0x20000000, value);
        }

        /// <summary>
        ///     MAG上昇
        /// </summary>
        public bool MagUp
        {
            get => Buffs[3].Test(0x40000000);
            set => Buffs[3].SetValue(0x40000000, value);
        }

        //#endregion

        //#region Buffs

        /// <summary>
        ///     狂战士
        /// </summary>
        public bool Berserker
        {
            get => Buffs[1].Test(0x1);
            set => Buffs[1].SetValue(0x1, value);
        }

        /// <summary>
        ///     诅咒
        /// </summary>
        public bool Curse
        {
            get => Buffs[1].Test(0x2);
            set => Buffs[1].SetValue(0x2, value);
        }

        /// <summary>
        ///     透视
        /// </summary>
        public bool Perspective
        {
            get => Buffs[1].Test(0x4);
            set => Buffs[1].SetValue(0x4, value);
        }

        /// <summary>
        ///     浮游
        /// </summary>
        public bool Float
        {
            get => Buffs[1].Test(0x8);
            set => Buffs[1].SetValue(0x8, value);
        }

        /// <summary>
        ///     水中呼吸
        /// </summary>
        public bool BreathingInWater
        {
            get => Buffs[1].Test(0x10);
            set => Buffs[1].SetValue(0x10, value);
        }

        /// <summary>
        ///     透明
        /// </summary>
        public bool Transparent
        {
            get => Buffs[1].Test(0x20);
            set => Buffs[1].SetValue(0x20, value);
        }

        /// <summary>
        ///     不死
        /// </summary>
        public bool Undead
        {
            get => Buffs[1].Test(0x40);
            set => Buffs[1].SetValue(0x40, value);
        }

        /// <summary>
        ///     蘑菇
        /// </summary>
        public bool Mushroom
        {
            get => Buffs[1].Test(0x80);
            set => Buffs[1].SetValue(0x80, value);
        }

        /// <summary>
        ///     Stiff
        /// </summary>
        public bool Stiff
        {
            get => Buffs[1].Test(0x100);
            set => Buffs[1].SetValue(0x100, value);
        }

        /// <summary>
        ///     咒缚
        /// </summary>
        public bool TheDamed
        {
            get => Buffs[1].Test(0x200);
            set => Buffs[1].SetValue(0x200, value);
        }

        /// <summary>
        ///     封印
        /// </summary>
        public bool Sealed
        {
            get => Buffs[1].Test(0x400);
            set => Buffs[1].SetValue(0x400, value);
        }

        /// <summary>
        ///     封魔
        /// </summary>
        public bool MagicSealed
        {
            get => Buffs[1].Test(0x800);
            set => Buffs[1].SetValue(0x800, value);
        }

        /// <summary>
        ///     准备PY Possession
        /// </summary>
        public bool GetReadyPossession
        {
            get => Buffs[1].Test(0x1000);
            set => Buffs[1].SetValue(0x1000, value);
        }

        /// <summary>
        ///     热波防御
        /// </summary>
        public bool HotGuard
        {
            get => Buffs[1].Test(0x2000);
            set => Buffs[1].SetValue(0x2000, value);
        }

        /// <summary>
        ///     寒波防御
        /// </summary>
        public bool ColdGuard
        {
            get => Buffs[1].Test(0x4000);
            set => Buffs[1].SetValue(0x4000, value);
        }

        /// <summary>
        ///     真空防御
        /// </summary>
        public bool VacuumGuard
        {
            get => Buffs[1].Test(0x8000);
            set => Buffs[1].SetValue(0x8000, value);
        }

        /// <summary>
        ///     猛毒
        /// </summary>
        public bool DeadlyPoison
        {
            get => Buffs[1].Test(0x10000);
            set => Buffs[1].SetValue(0x10000, value);
        }

        /// <summary>
        ///     神圣羽毛
        /// </summary>
        public bool HolyFeather
        {
            get => Buffs[1].Test(0x20000);
            set => Buffs[1].SetValue(0x20000, value);
        }

        /// <summary>
        ///     乌龟架势
        /// </summary>
        public bool ConstructionOfTheTurtle
        {
            get => Buffs[1].Test(0x40000);
            set => Buffs[1].SetValue(0x40000, value);
        }

        /// <summary>
        ///     必中阵
        /// </summary>
        public bool FormationOfDodgeless
        {
            get => Buffs[1].Test(0x80000);
            set => Buffs[1].SetValue(0x80000, value);
        }

        /// <summary>
        ///     短 剑延迟取消
        /// </summary>
        public bool ShortSwordDelayCancel
        {
            get => Buffs[1].Test(0x100000);
            set => Buffs[1].SetValue(0x100000, value);
        }

        /// <summary>
        ///     延迟取消
        /// </summary>
        public bool DelayCancel
        {
            get => Buffs[1].Test(0x200000);
            set => Buffs[1].SetValue(0x200000, value);
        }

        /// <summary>
        ///     斧延迟取消
        /// </summary>
        public bool AxeDelayCancel
        {
            get => Buffs[1].Test(0x400000);
            set => Buffs[1].SetValue(0x400000, value);
        }

        /// <summary>
        ///     矛延迟取消
        /// </summary>
        public bool SpearDelayCancel
        {
            get => Buffs[1].Test(0x800000);
            set => Buffs[1].SetValue(0x800000, value);
        }

        /// <summary>
        ///     弓延迟取消
        /// </summary>
        public bool BowDelayCancel
        {
            get => Buffs[1].Test(0x1000000);
            set => Buffs[1].SetValue(0x1000000, value);
        }

        /// <summary>
        ///     斩击抵抗
        /// </summary>
        public bool DefenseSlash
        {
            get => Buffs[1].Test(0x2000000);
            set => Buffs[1].SetValue(0x2000000, value);
        }

        /// <summary>
        ///     戳刺抵抗
        /// </summary>
        public bool DefenseStub
        {
            get => Buffs[1].Test(0x4000000);
            set => Buffs[1].SetValue(0x4000000, value);
        }

        /// <summary>
        ///     打击抵抗
        /// </summary>
        public bool DefenseBlow
        {
            get => Buffs[1].Test(0x8000000);
            set => Buffs[1].SetValue(0x8000000, value);
        }

        /// <summary>
        ///     再生
        /// </summary>
        public bool Revive
        {
            get => Buffs[1].Test(0x10000000);
            set => Buffs[1].SetValue(0x10000000, value);
        }

        /// <summary>
        ///     这是什么
        /// </summary>
        public bool PetUp
        {
            get => Buffs[1].Test(0x20000000);
            set => Buffs[1].SetValue(0x20000000, value);
        }

        /// <summary>
        ///     点火
        /// </summary>
        public bool Ignition
        {
            get => Buffs[1].Test(0x40000000);
            set => Buffs[1].SetValue(0x40000000, value);
        }

        //#endregion

        public bool 魂之手
        {
            get => Buffs[9].Test(0x1);
            set => Buffs[9].SetValue(0x1, value);
        }

        public bool 精准攻击
        {
            get => Buffs[9].Test(0x2);
            set => Buffs[9].SetValue(0x2, value);
        }

        public bool 恶炎
        {
            get => Buffs[9].Test(0x4);
            set => Buffs[9].SetValue(0x4, value);
        }

        public bool 九尾狐魅惑
        {
            get => Buffs[9].Test(0x8);
            set => Buffs[9].SetValue(0x8, value);
        }

        public bool 武装化
        {
            get => Buffs[9].Test(0x10);
            set => Buffs[9].SetValue(0x10, value);
        }

        public bool 武装化副作用
        {
            get => Buffs[9].Test(0x20);
            set => Buffs[9].SetValue(0x20, value);
        }

        public bool 恶魂
        {
            get => Buffs[9].Test(0x40);
            set => Buffs[9].SetValue(0x40, value);
        }

        //#region Buffs

        public bool Poison
        {
            get => Buffs[0].Test(0x1);
            set => Buffs[0].SetValue(0x1, value);
        }

        public bool Stone
        {
            get => Buffs[0].Test(0x2);
            set => Buffs[0].SetValue(0x2, value);
        }

        public bool Paralysis
        {
            get => Buffs[0].Test(0x4);
            set => Buffs[0].SetValue(0x4, value);
        }

        public bool Sleep
        {
            get => Buffs[0].Test(0x8);
            set => Buffs[0].SetValue(0x8, value);
        }

        public bool Silence
        {
            get => Buffs[0].Test(0x10);
            set => Buffs[0].SetValue(0x10, value);
        }

        public bool SpeedDown
        {
            get => Buffs[0].Test(0x20);
            set => Buffs[0].SetValue(0x20, value);
        }

        public bool Confused
        {
            get => Buffs[0].Test(0x40);
            set => Buffs[0].SetValue(0x40, value);
        }

        public bool Frosen
        {
            get => Buffs[0].Test(0x80);
            set => Buffs[0].SetValue(0x80, value);
        }

        public bool Stun
        {
            get => Buffs[0].Test(0x100);
            set => Buffs[0].SetValue(0x100, value);
        }

        public bool Dead
        {
            get => Buffs[0].Test(0x200);
            set => Buffs[0].SetValue(0x200, value);
        }

        public bool CannotMove
        {
            get => Buffs[0].Test(0x400);
            set => Buffs[0].SetValue(0x400, value);
        }

        public bool PoisonResist
        {
            get => Buffs[0].Test(0x800);
            set => Buffs[0].SetValue(0x800, value);
        }

        public bool StoneResist
        {
            get => Buffs[0].Test(0x1000);
            set => Buffs[0].SetValue(0x1000, value);
        }

        public bool ParalysisResist
        {
            get => Buffs[0].Test(0x2000);
            set => Buffs[0].SetValue(0x2000, value);
        }

        public bool SleepResist
        {
            get => Buffs[0].Test(0x4000);
            set => Buffs[0].SetValue(0x4000, value);
        }

        public bool SilenceResist
        {
            get => Buffs[0].Test(0x8000);
            set => Buffs[0].SetValue(0x8000, value);
        }

        public bool SpeedDownResist
        {
            get => Buffs[0].Test(0x10000);
            set => Buffs[0].SetValue(0x10000, value);
        }

        public bool ConfuseResist
        {
            get => Buffs[0].Test(0x20000);
            set => Buffs[0].SetValue(0x20000, value);
        }

        public bool FrosenResist
        {
            get => Buffs[0].Test(0x40000);
            set => Buffs[0].SetValue(0x40000, value);
        }

        public bool FaintResist
        {
            get => Buffs[0].Test(0x80000);
            set => Buffs[0].SetValue(0x80000, value);
        }

        public bool Sit
        {
            get => Buffs[0].Test(0x100000);
            set => Buffs[0].SetValue(0x100000, value);
        }

        public bool Spirit
        {
            get => Buffs[0].Test(0x200000);
            set => Buffs[0].SetValue(0x200000, value);
        }

        //#endregion
    }
}
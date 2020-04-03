using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Assistant.Core;

using Ultima;
using System.Linq;

namespace Assistant
{
    public class GumpResponseAction
    {
        private int m_ButtonID;
        public int Button => m_ButtonID;
        private int[] m_Switches;
        private GumpTextEntry[] m_TextEntries;

        public GumpResponseAction( string[] args )
        {
            m_ButtonID = Convert.ToInt32( args[1] );
            m_Switches = new int[Convert.ToInt32( args[2] )];
            for ( int i = 0; i < m_Switches.Length; i++ )
                m_Switches[i] = Convert.ToInt32( args[3 + i] );
            m_TextEntries = new GumpTextEntry[Convert.ToInt32( args[3 + m_Switches.Length] )];
            for ( int i = 0; i < m_TextEntries.Length; i++ )
            {
                string[] split = args[4 + m_Switches.Length + i].Split( '&' );
                m_TextEntries[i].EntryID = Convert.ToUInt16( split[0] );
                m_TextEntries[i].Text = split[1];
            }
        }

        public GumpResponseAction( int button, int[] switches, GumpTextEntry[] entries )
        {
            m_ButtonID = button;
            m_Switches = switches;
            m_TextEntries = entries;
        }

        public bool Perform()
        {
            ClientCommunication.SendToClient( new CloseGump( World.Player.CurrentGumpI ) );
            ClientCommunication.SendToServer( new GumpResponse( World.Player.CurrentGumpS, World.Player.CurrentGumpI, m_ButtonID, m_Switches, m_TextEntries ) );
            World.Player.HasGump = false;
            return true;
        }

      
        private void UseLastResponse( object[] args )
        {
            m_ButtonID = World.Player.LastGumpResponseAction.m_ButtonID;
            m_Switches = World.Player.LastGumpResponseAction.m_Switches;
            m_TextEntries = World.Player.LastGumpResponseAction.m_TextEntries;

            World.Player.SendMessage( MsgLevel.Force, "Set GumpResponse to last response" );

        }
    }

    public enum LockType : byte
    {
        Up = 0,
        Down = 1,
        Locked = 2
    }

    public enum MsgLevel
    {
        Debug = 0,
        Info = 0,
        Warning = 1,
        Error = 2,
        Force = 3
    }

    public class Skill
    {
        public static int Count = 55;

        private LockType m_Lock;
        private ushort m_Value;
        private ushort m_Base;
        private ushort m_Cap;
        private short m_Delta;
        private int m_Idx;

        public Skill(int idx)
        {
            m_Idx = idx;
        }

        public int Index { get { return m_Idx; } }

        public LockType Lock
        {
            get { return m_Lock; }
            set { m_Lock = value; }
        }

        public ushort FixedValue
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public ushort FixedBase
        {
            get { return m_Base; }
            set
            {
                m_Delta += (short)(value - m_Base);
                m_Base = value;
            }
        }

        public ushort FixedCap
        {
            get { return m_Cap; }
            set { m_Cap = value; }
        }

        public double Value
        {
            get { return m_Value / 10.0; }
            set { m_Value = (ushort)(value * 10.0); }
        }

        public double Base
        {
            get { return m_Base / 10.0; }
            set { m_Base = (ushort)(value * 10.0); }
        }

        public double Cap
        {
            get { return m_Cap / 10.0; }
            set { m_Cap = (ushort)(value * 10.0); }
        }

        public double Delta
        {
            get { return m_Delta / 10.0; }
            set { m_Delta = (short)(value * 10); }
        }
    }

    public enum SkillName
    {
        Alchemy = 0,
        Anatomy = 1,
        AnimalLore = 2,
        ItemID = 3,
        ArmsLore = 4,
        Parry = 5,
        Begging = 6,
        Blacksmith = 7,
        Fletching = 8,
        Peacemaking = 9,
        Camping = 10,
        Carpentry = 11,
        Cartography = 12,
        Cooking = 13,
        DetectHidden = 14,
        Discordance = 15,
        EvalInt = 16,
        Healing = 17,
        Fishing = 18,
        Forensics = 19,
        Herding = 20,
        Hiding = 21,
        Provocation = 22,
        Inscribe = 23,
        Lockpicking = 24,
        Magery = 25,
        MagicResist = 26,
        Tactics = 27,
        Snooping = 28,
        Musicianship = 29,
        Poisoning = 30,
        Archery = 31,
        SpiritSpeak = 32,
        Stealing = 33,
        Tailoring = 34,
        AnimalTaming = 35,
        TasteID = 36,
        Tinkering = 37,
        Tracking = 38,
        Veterinary = 39,
        Swords = 40,
        Macing = 41,
        Fencing = 42,
        Wrestling = 43,
        Lumberjacking = 44,
        Mining = 45,
        Meditation = 46,
        Stealth = 47,
        RemoveTrap = 48,
        Necromancy = 49,
        Focus = 50,
        Chivalry = 51,
        Bushido = 52,
        Ninjitsu = 53,
        SpellWeaving = 54
    }

    public enum MaleSounds
    {
        Ah = 0x41A,
        Ahha = 0x41B,
        Applaud = 0x41C,
        BlowNose = 0x41D,
        Burp = 0x41E,
        Cheer = 0x41F,
        ClearThroat = 0x420,
        Cough = 0x421,
        CoughBS = 0x422,
        Cry = 0x423,
        Fart = 0x429,
        Gasp = 0x42A,
        Giggle = 0x42B,
        Groan = 0x42C,
        Growl = 0x42D,
        Hey = 0x42E,
        Hiccup = 0x42F,
        Huh = 0x430,
        Kiss = 0x431,
        Laugh = 0x432,
        No = 0x433,
        Oh = 0x434,
        Oomph1 = 0x435,
        Oomph2 = 0x436,
        Oomph3 = 0x437,
        Oomph4 = 0x438,
        Oomph5 = 0x439,
        Oomph6 = 0x43A,
        Oomph7 = 0x43B,
        Oomph8 = 0x43C,
        Oomph9 = 0x43D,
        Oooh = 0x43E,
        Oops = 0x43F,
        Puke = 0x440,
        Scream = 0x441,
        Shush = 0x442,
        Sigh = 0x443,
        Sneeze = 0x444,
        Sniff = 0x445,
        Snore = 0x446,
        Spit = 0x447,
        Whistle = 0x448,
        Yawn = 0x449,
        Yea = 0x44A,
        Yell = 0x44B,
    }

    public enum FemaleSounds
    {
        Ah = 0x30B,
        Ahha = 0x30C,
        Applaud = 0x30D,
        BlowNose = 0x30E,
        Burp = 0x30F,
        Cheer = 0x310,
        ClearThroat = 0x311,
        Cough = 0x312,
        CoughBS = 0x313,
        Cry = 0x314,
        Fart = 0x319,
        Gasp = 0x31A,
        Giggle = 0x31B,
        Groan = 0x31C,
        Growl = 0x31D,
        Hey = 0x31E,
        Hiccup = 0x31F,
        Huh = 0x320,
        Kiss = 0x321,
        Laugh = 0x322,
        No = 0x323,
        Oh = 0x324,
        Oomph1 = 0x325,
        Oomph2 = 0x326,
        Oomph3 = 0x327,
        Oomph4 = 0x328,
        Oomph5 = 0x329,
        Oomph6 = 0x32A,
        Oomph7 = 0x32B,        
        Oooh = 0x32C,
        Oops = 0x32D,
        Puke = 0x32E,
        Scream = 0x32F,
        Shush = 0x330,
        Sigh = 0x331,
        Sneeze = 0x332,
        Sniff = 0x333,
        Snore = 0x334,
        Spit = 0x335,
        Whistle = 0x336,
        Yawn = 0x337,
        Yea = 0x338,
        Yell = 0x339,
    }

    public class PlayerData : Mobile
    {
        public int VisRange = 18;
        public int MultiVisRange { get { return VisRange + 5; } }

        private int m_MaxWeight = -1;

        private short m_FireResist, m_ColdResist, m_PoisonResist, m_EnergyResist, m_Luck;
        private ushort m_DamageMin, m_DamageMax;

        private ushort m_Str, m_Dex, m_Int;
        private LockType m_StrLock, m_DexLock, m_IntLock;
        private uint m_Gold;
        private ushort m_Weight;
        private Skill[] m_Skills;
        private ushort m_AR;
        private ushort m_StatCap;
        private byte m_Followers;
        private byte m_FollowersMax;
        private int m_Tithe;
        private sbyte m_LocalLight;
        private byte m_GlobalLight;
        private ushort m_Features;
        private byte m_Season;
        private byte m_DefaultSeason;
        private int[] m_MapPatches = new int[10];


        private bool m_SkillsSent;
        private DateTime m_CriminalStart = DateTime.MinValue;

        internal List<BuffsDebuffs> m_BuffsDebuffs = new List<BuffsDebuffs>();
        internal List<BuffsDebuffs> BuffsDebuffs { get { return m_BuffsDebuffs; } }

        private List<uint> m_OpenedCorpses = new List<uint>();
        public List<uint> OpenedCorpses { get { return m_OpenedCorpses; } }

        public Direction GetDirectionTo( int x, int y )
        {
            int dx = Position.m_X - x;
            int dy = Position.m_Y - y;

            int rx = ( dx - dy ) * 44;
            int ry = ( dx + dy ) * 44;

            int ax = Math.Abs( rx );
            int ay = Math.Abs( ry );

            Direction ret;

            if ( ( ( ay >> 1 ) - ax ) >= 0 )
                ret = ( ry > 0 ) ? Direction.Up : Direction.Down;
            else if ( ( ( ax >> 1 ) - ay ) >= 0 )
                ret = ( rx > 0 ) ? Direction.Left : Direction.Right;
            else if ( rx >= 0 && ry >= 0 )
                ret = Direction.West;
            else if ( rx >= 0 && ry < 0 )
                ret = Direction.South;
            else if ( rx < 0 && ry < 0 )
                ret = Direction.East;
            else
                ret = Direction.North;

            return ret;
        }

        public Direction GetDirectionTo( Point2D p )
        {
            return GetDirectionTo( p.m_X, p.m_Y );
        }

        public Direction GetDirectionTo( Point3D p )
        {
            return GetDirectionTo( p.m_X, p.m_Y );
        }

        public Direction GetDirectionTo( IPoint2D p )
        {
            if ( p == null )
                return Direction.North;

            return GetDirectionTo( p.X, p.Y );
        }
        public override void SaveState(BinaryWriter writer)
        {
            base.SaveState(writer);

            writer.Write(m_Str);
            writer.Write(m_Dex);
            writer.Write(m_Int);
            writer.Write(m_StamMax);
            writer.Write(m_Stam);
            writer.Write(m_ManaMax);
            writer.Write(m_Mana);
            writer.Write((byte)m_StrLock);
            writer.Write((byte)m_DexLock);
            writer.Write((byte)m_IntLock);
            writer.Write(m_Gold);
            writer.Write(m_Weight);

            writer.Write((byte)Skill.Count);
            for (int i = 0; i < Skill.Count; i++)
            {
                writer.Write(m_Skills[i].FixedBase);
                writer.Write(m_Skills[i].FixedCap);
                writer.Write(m_Skills[i].FixedValue);
                writer.Write((byte)m_Skills[i].Lock);
            }

            writer.Write(m_AR);
            writer.Write(m_StatCap);
            writer.Write(m_Followers);
            writer.Write(m_FollowersMax);
            writer.Write(m_Tithe);

            writer.Write(m_LocalLight);
            writer.Write(m_GlobalLight);
            writer.Write(m_Features);
            writer.Write(m_Season);

            writer.Write((byte)m_MapPatches.Length);
            for (int i = 0; i < m_MapPatches.Length; i++)
                writer.Write((int)m_MapPatches[i]);
        }

        public PlayerData(BinaryReader reader, int version) : base(reader, version)
        {
            int c;
            m_Str = reader.ReadUInt16();
            m_Dex = reader.ReadUInt16();
            m_Int = reader.ReadUInt16();
            m_StamMax = reader.ReadUInt16();
            m_Stam = reader.ReadUInt16();
            m_ManaMax = reader.ReadUInt16();
            m_Mana = reader.ReadUInt16();
            m_StrLock = (LockType)reader.ReadByte();
            m_DexLock = (LockType)reader.ReadByte();
            m_IntLock = (LockType)reader.ReadByte();
            m_Gold = reader.ReadUInt32();
            m_Weight = reader.ReadUInt16();

            if (version >= 4)
            {
                Skill.Count = c = reader.ReadByte();
            }
            else if (version == 3)
            {
                long skillStart = reader.BaseStream.Position;
                c = 0;
                reader.BaseStream.Seek(7 * 49, SeekOrigin.Current);
                for (int i = 48; i < 60; i++)
                {
                    ushort Base, Cap, Val;
                    byte Lock;

                    Base = reader.ReadUInt16();
                    Cap = reader.ReadUInt16();
                    Val = reader.ReadUInt16();
                    Lock = reader.ReadByte();

                    if (Base > 2000 || Cap > 2000 || Val > 2000 || Lock > 2)
                    {
                        c = i;
                        break;
                    }
                }

                if (c == 0)
                    c = 52;
                else if (c > 54)
                    c = 54;

                Skill.Count = c;

                reader.BaseStream.Seek(skillStart, SeekOrigin.Begin);
            }
            else
            {
                Skill.Count = c = 52;
            }

            m_Skills = new Skill[c];
            for (int i = 0; i < c; i++)
            {
                m_Skills[i] = new Skill(i);
                m_Skills[i].FixedBase = reader.ReadUInt16();
                m_Skills[i].FixedCap = reader.ReadUInt16();
                m_Skills[i].FixedValue = reader.ReadUInt16();
                m_Skills[i].Lock = (LockType)reader.ReadByte();
            }

            m_AR = reader.ReadUInt16();
            m_StatCap = reader.ReadUInt16();
            m_Followers = reader.ReadByte();
            m_FollowersMax = reader.ReadByte();
            m_Tithe = reader.ReadInt32();

            m_LocalLight = reader.ReadSByte();
            m_GlobalLight = reader.ReadByte();
            m_Features = reader.ReadUInt16();
            m_Season = reader.ReadByte();

            if (version >= 4)
                c = reader.ReadByte();
            else
                c = 8;
            m_MapPatches = new int[c];
            for (int i = 0; i < c; i++)
                m_MapPatches[i] = reader.ReadInt32();
        }

        public PlayerData(Serial serial) : base(serial)
        {
            m_Skills = new Skill[Skill.Count];
            for (int i = 0; i < m_Skills.Length; i++)
                m_Skills[i] = new Skill(i);
        }

        public ushort Str
        {
            get { return m_Str; }
            set { m_Str = value; }
        }

        public ushort Dex
        {
            get { return m_Dex; }
            set { m_Dex = value; }
        }

        public ushort Int
        {
            get { return m_Int; }
            set { m_Int = value; }
        }

        public uint Gold
        {
            get { return m_Gold; }
            set { m_Gold = value; }
        }

        public ushort Weight
        {
            get { return m_Weight; }
            set { m_Weight = value; }
        }

        public ushort MaxWeight
        {
            get
            {
                if (m_MaxWeight == -1)
                    return (ushort)((m_Str * 3.5) + 40);
                else
                    return (ushort)m_MaxWeight;
            }
            set
            {
                m_MaxWeight = value;
            }
        }

        public short FireResistance
        {
            get { return m_FireResist; }
            set { m_FireResist = value; }
        }

        public short ColdResistance
        {
            get { return m_ColdResist; }
            set { m_ColdResist = value; }
        }

        public short PoisonResistance
        {
            get { return m_PoisonResist; }
            set { m_PoisonResist = value; }
        }

        public short EnergyResistance
        {
            get { return m_EnergyResist; }
            set { m_EnergyResist = value; }
        }

        public short Luck
        {
            get { return m_Luck; }
            set { m_Luck = value; }
        }

        public ushort DamageMin
        {
            get { return m_DamageMin; }
            set { m_DamageMin = value; }
        }

        public ushort DamageMax
        {
            get { return m_DamageMax; }
            set { m_DamageMax = value; }
        }

        public LockType StrLock
        {
            get { return m_StrLock; }
            set { m_StrLock = value; }
        }

        public LockType DexLock
        {
            get { return m_DexLock; }
            set { m_DexLock = value; }
        }

        public LockType IntLock
        {
            get { return m_IntLock; }
            set { m_IntLock = value; }
        }

        public ushort StatCap
        {
            get { return m_StatCap; }
            set { m_StatCap = value; }
        }

        public ushort AR
        {
            get { return m_AR; }
            set { m_AR = value; }
        }

        public byte Followers
        {
            get { return m_Followers; }
            set { m_Followers = value; }
        }

        public byte FollowersMax
        {
            get { return m_FollowersMax; }
            set { m_FollowersMax = value; }
        }

        public int Tithe
        {
            get { return m_Tithe; }
            set { m_Tithe = value; }
        }

        public Skill[] Skills { get { return m_Skills; } }

        public bool SkillsSent
        {
            get { return m_SkillsSent; }
            set { m_SkillsSent = value; }
        }

      
        private void AutoOpenDoors()
        {
           
        }


        public override void OnPositionChanging(Point3D oldPos)
        {
 
            AutoOpenDoors();

            List<Mobile> mlist = new List<Mobile>(World.Mobiles.Values);
            for (int i = 0; i < mlist.Count; i++)
            {
                Mobile m = mlist[i];
                if (m != this)
                {
                    if (!Utility.InRange(m.Position, Position, VisRange))
                        m.Remove();
                    else
                        Targeting.CheckLastTargetRange(m);
                }
            }

            mlist = null;


            List<Item> ilist = new List<Item>(World.Items.Values);
            for (int i = 0; i < ilist.Count; i++)
            {
                Item item = ilist[i];
                if (item.Deleted || item.Container != null)
                    continue;

                int dist = Utility.Distance(item.GetWorldPosition(), Position);
                if (item != DragDropManager.Holding && (dist > MultiVisRange || (!item.IsMulti && dist > VisRange)))
                    item.Remove();
              }

            ilist = null;

            base.OnPositionChanging(oldPos);
        }

        public override void OnDirectionChanging(Direction oldDir)
        {
            AutoOpenDoors();
        }

        public override void OnMapChange(byte old, byte cur)
        {
            List<Mobile> list = new List<Mobile>(World.Mobiles.Values);
            for (int i = 0; i < list.Count; i++)
            {
                Mobile m = list[i];
                if (m != this && m.Map != cur)
                    m.Remove();
            }

            list = null;

            World.Items.Clear();
            for (int i = 0; i < Contains.Count; i++)
            {
                Item item = (Item)Contains[i];
                World.AddItem(item);
                item.Contains.Clear();
            }

            if (Config.GetBool("AutoSearch") && Backpack != null)
                PlayerData.DoubleClick(Backpack);
        }

        /*public override void OnMapChange( byte old, byte cur )
        {
             World.Mobiles.Clear();
             World.Items.Clear();
             Counter.Reset();

             Contains.Clear();

             World.AddMobile( this );

             UOAssist.PostMapChange( cur );
        }*/

        protected override void OnNotoChange(byte old, byte cur)
        {
          
        }

       

      

        internal void SendMessage(MsgLevel lvl, LocString loc, params object[] args)
        {
            SendMessage(lvl, Language.Format(loc, args));
        }

        internal void SendMessage(MsgLevel lvl, LocString loc)
        {
            SendMessage(lvl, Language.GetString(loc));
        }

        internal void SendMessage(LocString loc, params object[] args)
        {
            SendMessage(MsgLevel.Info, Language.Format(loc, args));
        }

        internal void SendMessage(LocString loc)
        {
            SendMessage(MsgLevel.Info, Language.GetString(loc));
        }

        /*internal void SendMessage( int hue, LocString loc, params object[] args )
        {
             SendMessage( hue, Language.Format( loc, args ) );
        }*/

        internal void SendMessage(MsgLevel lvl, string format, params object[] args)
        {
            SendMessage(lvl, String.Format(format, args));
        }

        internal void SendMessage(string format, params object[] args)
        {
            SendMessage(MsgLevel.Info, String.Format(format, args));
        }

        internal void SendMessage(string text)
        {
            SendMessage(MsgLevel.Info, text);
        }

        internal void SendMessage(MsgLevel lvl, string text)
        {
            if (lvl >= (MsgLevel)Config.GetInt("MessageLevel") && text.Length > 0)
            {
                int hue;
                switch (lvl)
                {
                    case MsgLevel.Error:
                    case MsgLevel.Warning:
                        hue = Config.GetInt("WarningColor");
                        break;

                    default:
                        hue = Config.GetInt("SysColor");
                        break;
                }

                ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, hue, 3, Language.CliLocName, "System", text));

                PacketHandlers.SysMessages.Add(text);

                if (PacketHandlers.SysMessages.Count >= 25)
                    PacketHandlers.SysMessages.RemoveRange(0, 10);
            }
        }

        public uint CurrentGumpS, CurrentGumpI;
        public GumpResponseAction LastGumpResponseAction;
        public bool HasGump;
        public List<string> CurrentGumpStrings = new List<string>();
        public string CurrentGumpRawData;
        public uint CurrentMenuS;
        public ushort CurrentMenuI;
        public bool HasMenu;

        private ushort m_SpeechHue;
        public ushort SpeechHue { get { return m_SpeechHue; } set { m_SpeechHue = value; } }

        public sbyte LocalLightLevel { get { return m_LocalLight; } set { m_LocalLight = value; } }
        public byte GlobalLightLevel { get { return m_GlobalLight; } set { m_GlobalLight = value; } }

        public enum SeasonFlag
        {
            Spring,
            Summer,
            Fall,
            Winter,
            Desolation
        }
      

        public ushort Features { get { return m_Features; } set { m_Features = value; } }
        public int[] MapPatches { get { return m_MapPatches; } set { m_MapPatches = value; } }

        private int m_LastSkill = -1;
        public int LastSkill { get { return m_LastSkill; } set { m_LastSkill = value; } }

        private Serial m_LastObj = Serial.Zero;
        public Serial LastObject { get { return m_LastObj; } set { m_LastObj = value; } }

        public IUOEntity LastObjectAsEntity => World.FindEntity( LastObject );
        

        private int m_LastSpell = -1;
        public int LastSpell { get { return m_LastSpell; } set { m_LastSpell = value; } }

        public byte WalkSequence { get; internal set; }
        public Item LastContainer { get; internal set; }
        public DateTime LastContainerOpenedAt { get; internal set; } = DateTime.UtcNow;
        public DateTime LastGumpOpenedAt { get; internal set; } = DateTime.UtcNow;
        public uint LastGumpX { get; internal set; }
        public uint LastGumpY { get; internal set; }
        public int LastGumpWidth { get; internal set; }
        public int LastGumpHeight { get; internal set; }
        public string LastSystemMessage => PacketHandlers.SysMessages.Last();
        private List<string> m_Journal = new List<string>();
        public uint LastContainerGumpGraphic { get; internal set; }
        public List<uint> IgnoredItems = new List<uint>();
        public List<ushort> IgnoredTypes = new List<ushort>();

        //private UOEntity m_LastCtxM = null;
        //public UOEntity LastContextMenu { get { return m_LastCtxM; } set { m_LastCtxM = value; } }

        public void ClearJournal()
        {
            m_Journal.Clear();
        }

        public string GetJournal(int index)
        {
            if(m_Journal.Count > index)
                return m_Journal[index];
            return string.Empty;
        }

        public void AddToJournal(string msg )
        {
            m_Journal.Insert( 0, msg );
            if ( m_Journal.Count > 500 )
                m_Journal.RemoveAt( 500 );
        }

        public static bool DoubleClick(object clicked)
        {
            return DoubleClick(clicked, true);
        }

        public static bool DoubleClick(object clicked, bool silent)
        {
            Serial s;
            if (clicked is Mobile)
                s = ((Mobile)clicked).Serial.Value;
            else if (clicked is Item)
                s = ((Item)clicked).Serial.Value;
            else if (clicked is Serial)
                s = ((Serial)clicked).Value;
            else
                s = Serial.Zero;

            if (s != Serial.Zero)
            {
                Item free = null, pack = World.Player.Backpack;
                if (s.IsItem && pack != null && Config.GetBool("PotionEquip"))
                {
                    Item i = World.FindItem(s);
                    if (i != null && i.IsPotion && i.ItemID != 3853) // dont unequip for exploison potions
                    {
                        // dont worry about uneqipping RuneBooks or SpellBooks
                        Item left = World.Player.GetItemOnLayer(Layer.LeftHand);
                        Item right = World.Player.GetItemOnLayer(Layer.RightHand);

                        if (left != null && (right != null || left.IsTwoHanded))
                            free = left;
                        else if (right != null && right.IsTwoHanded)
                            free = right;

                        if (free != null)
                        {
                            if (DragDropManager.HasDragFor(free.Serial))
                                free = null;
                            else
                                DragDropManager.DragDrop(free, pack);
                        }
                    }
                }

                ActionQueue.DoubleClick(silent, s);

                if (free != null)
                    DragDropManager.DragDrop(free, World.Player, free.Layer, true);

                if (s.IsItem)
                    World.Player.m_LastObj = s;
            }

            return false;
        }
    }
}

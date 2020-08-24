﻿using System;
using System.Collections.Generic;
namespace FallGuysStats {
    public class RoundInfo {
        public string Name { get; set; }
        public int ShowID { get; set; }
        public int Round { get; set; }
        public int Position { get; set; }
        public int? Score { get; set; }
        public int Tier { get; set; }
        public bool Qualified { get; set; }
        public int Kudos { get; set; }
        public int Players { get; set; }
        public DateTime Start { get; set; } = DateTime.MinValue;
        public DateTime End { get; set; } = DateTime.MinValue;

        public void ToLocalTime() {
            Start = Start.Add(Start - Start.ToUniversalTime());
            End = End.Add(End - End.ToUniversalTime());
        }
        public override string ToString() {
            return $"{Name}: Round={Round} Position={Position} Duration={End - Start} Kudos={Kudos}";
        }
    }
    public enum LevelType {
        Race,
        Survival,
        Team,
        Final
    }
    public class LevelStats {
        public static Dictionary<string, string> DisplayNameLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
            { "round_door_dash",                  "Door Dash" },
            { "round_gauntlet_02",                "Dizzy Heights" },
            { "round_dodge_fall",                 "Fruit Chute" },
            { "round_chompchomp",                 "Gate Crash" },
            { "round_gauntlet_01",                "Hit Parade" },
            { "round_see_saw",                    "See Saw" },
            { "round_lava",                       "Slime Climb" },
            { "round_tip_toe",                    "Tip Toe" },
            { "round_gauntlet_03",                "Whirlygig" },

            { "round_block_party",                "Block Party" },
            { "round_jump_club",                  "Jump Club" },
            { "round_match_fall",                 "Perfect Match" },
            { "round_tunnel",                     "Roll Out" },
            { "round_tail_tag",                   "Tail Tag" },

            { "round_egg_grab",                   "Egg Scramble" },
            { "round_fall_ball_60_players",       "Fall Ball" },
            { "round_ballhogs",                   "Hoarders" },
            { "round_hoops",                      "Hoopsie Daisy" },
            { "round_jinxed",                     "Jinxed" },
            { "round_rocknroll",                  "Rock'N'Roll" },
            { "round_conveyor_arena",             "Team Tail Tag" },

            { "round_fall_mountain_hub_complete", "Fall Mountain" },
            { "round_floor_fall",                 "Hex-A-Gone" },
            { "round_jump_showdown",              "Jump Showdown" },
            { "round_royal_rumble",               "Royal Fumble" },
        };

        public string Name { get; set; }
        public int Qualified { get; set; }
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Bronze { get; set; }
        public int Played { get; set; }
        public int Kudos { get; set; }
        public int AveKudos { get { return Kudos / (Played == 0 ? 1 : Played); } }
        public LevelType Type;
        public TimeSpan AveDuration { get { return TimeSpan.FromSeconds((int)Duration.TotalSeconds / (Played == 0 ? 1 : Played)); } }
        public TimeSpan Duration;
        public string LevelName;
        public List<RoundInfo> Stats;

        public LevelStats(string levelName, LevelType type) {
            string name = null;
            if (DisplayNameLookup.TryGetValue(levelName, out name)) {
                Name = name;
            } else {
                Name = levelName;
            }
            LevelName = levelName;
            Type = type;
            Stats = new List<RoundInfo>();
            Clear();
        }
        public void Clear() {
            Qualified = 0;
            Gold = 0;
            Silver = 0;
            Bronze = 0;
            Played = 0;
            Kudos = 0;
            Duration = TimeSpan.Zero;
            Stats.Clear();
        }
        public void Add(RoundInfo stat) {
            Stats.Add(stat);
            Played++;
            if (stat.Tier == 1) {
                Gold++;
            } else if (stat.Tier == 2) {
                Silver++;
            } else if (stat.Tier == 3) {
                Bronze++;
            }
            Kudos += stat.Kudos;
            Duration += stat.End - stat.Start;
            Qualified += stat.Qualified ? 1 : 0;
        }

        public override string ToString() {
            return $"{Name}: {Qualified} / {Played}";
        }
    }
}
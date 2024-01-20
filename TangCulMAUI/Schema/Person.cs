using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using TangCulMAUI.Schema.InternalData;


namespace TangCulMAUI.Schema
{
    public readonly struct PersonSetting
    {
        public readonly Dictionary<string, PersonTraitInfo> Traits;
        public readonly int dice_age, disease, scope_dice;
        public readonly int[] die_probability, die_age;
        public PersonSetting(JObject Pairs)
        {
            dice_age = (int)(Pairs["dice_age"] ?? 0);
            disease = (int)(Pairs["disease"] ?? 0);
            scope_dice = (int)(Pairs["scope_dice"] ?? 0);
            die_probability = ((JArray)Pairs["die_probability"]).ToObject<int[]>() ?? [];
            die_age = ((JArray)Pairs["die_age"]).ToObject<int[]>() ?? [];
            Traits = ParsingTraits((JObject)Pairs["Traits"]);
            // JSON에서 여기로 저장.
        }

        static Dictionary<string, PersonTraitInfo> ParsingTraits(JObject pairs)
        {
            Dictionary<string, PersonTraitInfo> ret = new();
            foreach (var pair in pairs)
            {
                if (pair.Value == null) continue;
                ret[pair.Key] = new PersonTraitInfo((JObject)pair.Value);
            }

            return ret;
        }
    }
    public readonly struct PersonTraitInfo
    {
        public readonly int? disease, death_p;
        public readonly int[]? die_probability, die_age;
        public readonly string type;

        public PersonTraitInfo(JObject Info)
        {
            if (Info["disease"] != null) disease = (int)Info["disease"];
            if (Info["death_p"] != null) death_p = (int)Info["death_p"];
            if (Info["type"] != null) type = (string)Info["type"];
            if (Info["die_probability"] != null) die_probability = ((JArray)Info["die_probability"]).ToObject<int[]>();
            if (Info["die_age"] != null) die_age = ((JArray)Info["die_age"]).ToObject<int[]>();
        }
    }
    public enum PersonStatus
    {
        Alive,
        Sick,
        Dead
    }
    public class PersonData()
    {

    }

    public class Person(string _name, int _age, string[] _trait, PersonStatus _st_die, string _agent)
    {
        public string? Name { get; set; } = _name;
        public int Age { get; set; } = _age;
        public string[]? Traits { get; set; } = _trait;
        public string TraitsToShow { get {
                if (Traits == null) return "";
                string show = "";
                for(int i = 0; i < Traits.Length; i++)
                {
                    show += Traits[i]; 
                    if(i!= Traits.Length-1) show += ", ";
                }
                return show;
            } 
        }

        public int StatusToDie { get; set; }
        public int DicePoint { get; set; }
        public bool IsDead { get; set; }
        public PersonStatus Status { get; set; } = _st_die;
        public string? Agent { get; set; } = _agent;
        public string DisplayStatus
        {
            get
            {
                return Status switch
                {
                    PersonStatus.Alive => "생존",
                    PersonStatus.Sick => "아픔",
                    PersonStatus.Dead => "사망",
                    _ => "",
                };
            }
            set
            {
                switch (value)
                {
                    case "생존": Status = PersonStatus.Alive; break;
                    case "아픔": Status = PersonStatus.Sick; break;
                    case "사망": Status = PersonStatus.Dead; break;
                }
            }
        }

        /// <summary>
        /// 사망등을 결정하는 Dice를 굴리고, 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int Dice(bool mode, PersonSetting setting)
        {
            if (Status == PersonStatus.Dead && !mode) return 0;

            Random rand = new();

            int dice = Age < setting.dice_age ? GetMaxDice() : rand.Next(0, 1000);
            if (!mode) Age++;

            int diseases = 0;
            int diseases_count = 1;
            int dis = setting.disease;
            Dictionary<string, PersonTraitInfo> trait_info = setting.Traits;
            int[] diepercent = setting.die_probability;
            int[] dieage = setting.die_age;
            int die = GetDiePoint(Age, dieage, diepercent);

            if (Traits != null)
            {
                foreach (string trait in Traits)
                {
                    if (trait == null) continue;

                    if (trait_info.TryGetValue(trait, out PersonTraitInfo tra_obj) != false)
                    {
                        int perscope = Age < setting.dice_age ? 1 : setting.scope_dice;

                        if (tra_obj.type == "unique")
                        {
                            if (tra_obj.die_age != null && tra_obj.die_probability != null)
                            {
                                dieage = tra_obj.die_age;
                                diepercent = tra_obj.die_probability;
                                die = diepercent[^1];

                            }
                            if (tra_obj.disease != null)
                            {
                                diseases += (int)tra_obj.disease * perscope;
                                diseases_count++;

                            }
                            if (tra_obj.death_p != null)
                            {
                                die += (int)tra_obj.death_p * perscope;
                            }
                        }
                        else
                        {
                            if (tra_obj.death_p != null)
                            {
                                die += (int)tra_obj.death_p * perscope;
                            }
                        }
                    }

                }
            }
            Status = dice > die + dis ? PersonStatus.Dead :
                 dice > dis ? PersonStatus.Sick :
                 PersonStatus.Alive;
            DicePoint = dice;
            Thread.Sleep(12);
            return dice;
        }

        static int GetMaxDice()
        {
            Random rand = new();
            int dice1v = rand.Next(1, 100);
            rand = new();
            int dice2v = rand.Next(1, 100);
            return Math.Max(dice1v, dice2v);
        }

        static int GetDiePoint(int age, int[] agearr, int[] pointarr)
        {

            int v = pointarr[^1];
            for (int i = 0; i < agearr.Length; i++)
            {
                if (age < agearr[i])
                {
                    return pointarr[i];
                }

            }
            return v;
        }

        public JObject SavePersonToJsonObject()
        {
            JObject ret = new();
            try
            {
                ret["name"] = Name ?? "error";
                ret["age"] = Age;
                switch (Status)
                {
                    case PersonStatus.Alive:
                        ret["state_die"] = 2;
                        break;
                    case PersonStatus.Sick:
                        ret["state_die"] = 1;
                        break;
                    case PersonStatus.Dead:
                        ret["state_die"] = 0;
                        break;
                }
                ret["trait"] = new JArray(Traits ?? []);
                ret["agent"] = Agent;
                return ret;
            } 
            catch (Exception ex)
            {

                return new();
            }
        }
        public static explicit operator DB.DBPersonData(Person origin)
        {
            DB.DBPersonData ret = new()
            {
                Name = origin.Name,
                Traits = String.Join('\t',origin.Traits),
                Status = origin.Status,
                Agent = origin.Agent,
                Age = origin.Age,
            };
            return ret;
        }
    }



}
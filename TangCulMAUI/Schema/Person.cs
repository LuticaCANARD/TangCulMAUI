using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace TangCulMAUI.Schema
{
    public struct PersonSetting
    {
        Dictionary<string, object> setting;
        public PersonSetting(JObject Pairs) { 

            setting = new Dictionary<string, object>();
            foreach (var item in Pairs)
            {

                // 속성 지정해서 보내기.

            }
            // JSON에서 여기로 저장.
        }
    }
    public enum PersonStatus
    {
        Alive,
        Sick,
        Dead
    }
    public class Person
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public string[]? Traits { get; set; }
        public int StatusToDie {  get; set; }
        public int DicePoint { get; set; }
        public bool IsDead { get; set; }
        public PersonStatus Status { get; set; }
        public string? Agent { get; set; }

        public Person(string _name, int _age, string[] _trait, PersonStatus _st_die, string _agent) 
        {
            this.Name = _name;
            this.Traits = _trait;
            this.Age = _age;
            this.Status = _st_die;
            this.Agent = _agent;
        }
        /// <summary>
        /// 사망등을 결정하는 Dice를 굴리고, 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int Dice(bool mode,Dictionary<string, object> setting)
        {
            if(Status == PersonStatus.Dead && !mode)
            {
                return 0;
            }
            Random rand = new();

            int dice = Age < (int)setting["dice_age"] ? GetMaxDice() : rand.Next(0, 1000);
            if (!mode) Age ++;

            int diseases = 0;
            int diseases_count = 1;
            int dis = (int)setting["disease"];


            if (Traits != null)
            {
                Dictionary<string, object> trait_info = (Dictionary<string, object>)setting["trait"];
                foreach (string trait in Traits)
                {
                    if (trait == null) continue;
                    int perscope;
                    perscope = Age < (int)setting["dice_age"] ? 1 : (int)setting["scope_dice"];
                    if (trait_info[trait] != null)
                    {

                    }
               
                }
            }



            int die = GetDiePoint(Age, (int[])setting["die_age"], (int[])setting["die_probability"]);
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

        static int GetDiePoint(int age,int[] agearr, int[] pointarr)
        {

            int v = pointarr[pointarr.Length - 1];
            for (int i = 0; i < agearr.Length; i++)
            {
                if (age < (int)agearr[i])
                {
                    return pointarr[i];
                }

            }
            return v;
        }

    }
    public class Person____
    {

        public string name { get; set; }
        public int age { get; set; }
        public string[] traits { get; set; }
        public int st_die { get; set; }
        public JObject book { get; set; }
        public int dicek { get; set; }
        public string agent { get; set; }

        public Person____(string _name, int _age, string[] _trait, int _st_die, JObject keyValues, string _agent)
        {
            this.name = _name;
            this.traits = _trait;
            this.age = _age;
            this.st_die = _st_die;
            this.book = keyValues;
            this.agent = _agent;
        }
        public int dice(bool mode)
        {
            if (this.st_die == 0 && !mode) return 0;
            int age = this.age;
            Random rand = new Random();

            int dis = (int)book["disease"];
            JArray diepercent = (JArray)book["die_probability"];
            JArray dieage = (JArray)book["die_age"];
            JObject trait = (JObject)book["trait"];
            int die = (int)diepercent[diepercent.Count - 1];

            int dicef;

            if (age < (int)book["dice_age"])
            {
                int dice1v = rand.Next(1, 100);
                rand = new Random();

                int dice2v = rand.Next(1, 100);
                dicef = Math.Max(dice1v, dice2v);
            }
            else
            { dicef = rand.Next(0, 1000); }
            int diseases = 0;
            int dis_count = 1;
            foreach (string tra in this.traits)
            {
                int perscope;
                if (age < (int)book["dice_age"])
                    perscope = 1;
                else
                    perscope = (int)book["scope_dice"];

                if (trait[tra] != null)
                {

                    JObject tra_obj = (JObject)trait[tra];
                    if (tra_obj["scope_dice"] != null)
                    {
                        if (age < (int)book["dice_age"])
                        {
                            perscope = 1;
                        }
                        else
                        {
                            perscope = (int)tra_obj["scope_dice"];
                        }
                    }
                    if (tra_obj["type"].ToString() == "unique")
                    {
                        if (tra_obj["die_age"] != null)
                        {
                            dieage = (JArray)tra_obj["die_age"];
                            diepercent = (JArray)tra_obj["die_probability"];
                            die = (int)diepercent[diepercent.Count - 1];

                        }
                        if (tra_obj["disease"] != null)
                        {
                            diseases += int.Parse(tra_obj["disease"].ToString()) * perscope;
                            dis_count++;

                        }
                        if (tra_obj["death_p"] != null)
                        {
                            die += (int)tra_obj["death_p"] * perscope;
                        }
                    }
                    else
                    {
                        if (tra_obj["death_p"] != null)
                        {
                            die += (int)tra_obj["death_p"] * perscope;
                        }
                    }
                }
            }
            dis = (dis + diseases) / dis_count;


            if (!mode) this.age += 1;

            for (int i = 0; i < dieage.Count; i++)
            {
                if (age < (int)dieage[i])
                {
                    die = (int)diepercent[i];
                    break;
                }

            }
            if (dicef > die + dis) this.st_die = 2;
            else if (dicef > die) this.st_die = 1;
            else this.st_die = 0;
            this.dicek = dicef;
            Thread.Sleep(12);
            return dicef;
        }


    }
}
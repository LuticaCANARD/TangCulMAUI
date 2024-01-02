using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace TangCulMAUI.Schema
{
    public class Person
    {

        public string name { get; set; }
        public int age { get; set; }
        public string[] traits { get; set; }
        public int st_die { get; set; }
        public JObject book { get; set; }
        public int dicek { get; set; }
        public string agent { get; set; }

        public Person(string _name, int _age, string[] _trait, int _st_die, JObject keyValues, string _agent)
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
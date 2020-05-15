using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class SkillSystem : ISystem
    {

        private List<SkillComponent> _skillComponentsList = new List<SkillComponent>();
        public enum SkillType { HEALTH, ATTACKSPEED};

        private static SkillSystem instance = null;
        private SkillSystem() { }
        public static SkillSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SkillSystem();
                }
                return instance;
            }
        }


        internal IEntityComponent CreateSkillComponent()
        {
            SkillComponent skillComponent = new SkillComponent(this);
            _skillComponentsList.Add(skillComponent);
            return skillComponent;
        }


        public void Init()
        {
            EventSystem.Instance.AddListener("IncreaseSkill", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) =>
                {
                    if(parameters[0] is GameObject && parameters[1] is SkillType)
                    {
                        IncreaseSkill((GameObject)parameters[0], (SkillType)parameters[1]);
                    } else
                    {
                        Console.WriteLine("Parameters are not compatible with Event: \"IncreaseSkill\" ");
                    }
                }
            });
        }

        public void IncreaseSkill(GameObject go_1, SkillType skillType)
        {
            int skillValue = 0;
            string skillName = "";

            SkillComponent component = go_1.GetComponent<SkillComponent>();

            switch (skillType)
            {
                case SkillType.HEALTH:
                    skillName = "Health";
                    break;
                case SkillType.ATTACKSPEED:
                    skillName = "Attackspeed";
                    break;
                default:
                    break;
            }

            if (component.Skills.TryGetValue(skillName, out skillValue))
            {
                component.Skills[skillName] = skillValue + 1;
                if (skillType == SkillType.HEALTH)
                {
                    EventSystem.Instance.RaiseEvent("IncreaseHealth", 100 + 10 * component.Skills[skillName]);
                }
                else if (skillType == SkillType.ATTACKSPEED)
                {
                    EventSystem.Instance.RaiseEvent("IncreaseAttackspeed", 50 * component.Skills[skillName]);
                }
                Console.WriteLine("VALUE: " + component.Skills[skillName]);
            }
            else
            {
                Console.WriteLine("Skill not found");
            }
        }
    }
}
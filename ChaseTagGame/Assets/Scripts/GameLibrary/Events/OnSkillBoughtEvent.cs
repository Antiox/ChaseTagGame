using UnityEngine;

namespace GameLibrary
{
    public class OnSkillBoughtEvent : IGameEvent
    {
        public Skill Skill { get; set; }
        public GameObject SkillPanel { get; set; }
        public int NewCurrency { get; set; }

        public OnSkillBoughtEvent(Skill s, GameObject panel, int currency)
        {
            Skill = s;
            SkillPanel = panel;
            NewCurrency = currency;
        }
    }
}

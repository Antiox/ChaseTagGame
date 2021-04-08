using UnityEngine;

namespace GameLibrary
{
    public class OnSkillBoughtEvent : IGameEvent
    {
        public Skill Skill { get; set; }
        public GameObject SkillPanel { get; set; }

        public OnSkillBoughtEvent(Skill s, GameObject panel)
        {
            Skill = s;
            SkillPanel = panel;
        }
    }
}

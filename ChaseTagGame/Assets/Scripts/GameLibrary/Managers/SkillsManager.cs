using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class SkillsManager
    {
        public List<Skill> PossibleSkills { get { return GetRandomSkills(); } }
        public List<Skill> OwnedSkills { get; private set; }

        #region Singleton
        private static SkillsManager instance;
        public static SkillsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SkillsManager();
                return instance;
            }
        }
        #endregion

        private SkillsManager()
        {
            OwnedSkills = new List<Skill>();
        }

        public void Start()
        {
            EventManager.Instance.AddListener<OnSkillBoughtEvent>(OnSkillBought);
        }

        public void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnSkillBoughtEvent>(OnSkillBought);
        }

        public void Reset()
        {
            OwnedSkills = new List<Skill>();
        }

        private Skill CreateSkill(SkillType type)
        {
            return Resources.Load<Skill>($"Scriptables/{type}Skill");
        }

        private List<Skill> GetRandomSkills()
        {
            var skills = new List<Skill>();

            for (int i = 0; i < Enum.GetNames(typeof(SkillType)).Length; i++)
            {
                if (UnityEngine.Random.value >= 0.5f || skills.Count == 0)
                {
                    var s = CreateSkill((SkillType)i);
                    if (!OwnedSkills.Contains(s))
                        skills.Add(s);
                }
            }

            return skills;
        }

        private void OnSkillBought(OnSkillBoughtEvent e)
        {
            OwnedSkills.Add(e.Skill);
        }
    }

    public enum SkillType
    {
        Jump,
        Climb,
        Magnet,
        Slide,
        Run,
    }
}

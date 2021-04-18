using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace GameLibrary
{
    public class MagnetScript : MonoBehaviour
    {
        private bool canUseMagnet;
        private float attractionSpeed;


        private void Start()
        {
            canUseMagnet = GameManager.IsOwningSkill(SkillType.Magnet);
            attractionSpeed = 6f;
            EventManager.Instance.AddListener<OnSkillBoughtEvent>(OnSkillBought);
        }

        private void FixedUpdate()
        {
            if(canUseMagnet)
            {
                var colliders = Physics.OverlapSphere(transform.position + Vector3.up, 3f);
                foreach (var c in colliders.Where(p => p.CompareTag(GameTags.PowerUp) || p.CompareTag(GameTags.Objective) || p.CompareTag(GameTags.Gem)))
                    c.transform.position = Vector3.MoveTowards(c.transform.position, transform.position + Vector3.up, Time.deltaTime * attractionSpeed);
            }
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnSkillBoughtEvent>(OnSkillBought);
        }


        private void OnSkillBought(OnSkillBoughtEvent e)
        {
            canUseMagnet = e.Skill.type == SkillType.Magnet;
        }
    }
}

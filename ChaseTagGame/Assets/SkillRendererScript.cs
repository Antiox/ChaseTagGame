using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GameLibrary
{
    public class SkillRendererScript : MonoBehaviour
    {
        [SerializeField] private Skill skill;
        [SerializeField] private TextMeshProUGUI skillNameLabel;
        [SerializeField] private Image skillArtwork;


        private void Start()
        {
            skillNameLabel.text = skill.name;
            skillArtwork.sprite = skill.artwork;
        }
    }
}
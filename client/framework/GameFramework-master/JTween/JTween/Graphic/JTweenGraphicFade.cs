﻿using Json;
using DG.Tweening;
using UnityEngine;

namespace JTween.Graphic {
    public class JTweenGraphicFade : JTweenBase {
        private Color m_beginColor = Color.white;
        private float m_toAlpha = 0;
        private UnityEngine.UI.Graphic m_Graphic;

        public JTweenGraphicFade() {
            m_tweenType = (int)JTweenGraphic.Fade;
            m_tweenElement = JTweenElement.Graphic;
        }

        public Color BeginColor {
            get {
                return m_beginColor;
            }
            set {
                m_beginColor = value;
            }
        }

        public float ToAlpha {
            get {
                return m_toAlpha;
            }
            set {
                m_toAlpha = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Graphic = m_target.GetComponent<UnityEngine.UI.Graphic>();
            if (null == m_Graphic) return;
            // end if
            m_beginColor = m_Graphic.color;
        }

        protected override Tween DOPlay() {
            if (null == m_Graphic) return null;
            // end if
            return m_Graphic.DOFade(m_toAlpha, m_duration);
        }

        public override void Restore() {
            if (null == m_Graphic) return;
            // end if
            m_Graphic.color = m_beginColor;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginColor")) BeginColor = JTweenUtils.JsonToColor(json.GetNode("beginColor"));
            // end if
            if (json.Contains("alpha")) m_toAlpha = json.GetFloat("alpha");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginColor", JTweenUtils.ColorJson(m_beginColor));
            json.SetFloat("alpha", m_toAlpha);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Graphic) {
                errorInfo = GetType().FullName + " GetComponent<Graphic> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Graphic {
    public class JTweenGraphicBlendableColor : JTweenBase {
        private Color m_beginColor = Color.white;
        private Color m_toColor = Color.white;
        private UnityEngine.UI.Graphic m_Graphic;

        public JTweenGraphicBlendableColor() {
            m_tweenType = (int)JTweenGraphic.BlendableColor;
            m_tweenElement = JTweenElement.Graphic;
        }

        public Color BeginColor {
            get {
                return m_beginColor;
            }
            set {
                m_beginColor = value;
                if (m_Graphic != null) {
                    m_Graphic.color = m_beginColor;
                } // end if
            }
        }

        public Color ToColor {
            get {
                return m_toColor;
            }
            set {
                m_toColor = value;
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
            return m_Graphic.DOBlendableColor(m_toColor, m_duration);
        }

        public override void Restore() {
            if (null == m_Graphic) return;
            // end if
            m_Graphic.color = m_beginColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginColor")) BeginColor = JTweenUtils.JsonToColor(json["beginColor"]);
            // end if
            if (json.Contains("color")) m_toColor = JTweenUtils.JsonToColor(json["color"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginColor"] = JTweenUtils.ColorJson(m_beginColor);
            json["color"] = JTweenUtils.ColorJson(m_toColor);
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
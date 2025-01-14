﻿using Json;
using DG.Tweening;
using UnityEngine;

namespace JTween.Material {
    public class JTweenMaterialColor : JTweenBase {
        private Color m_beginColor = Color.white;
        private Color m_toColor = Color.white;
        private string m_property = string.Empty;
        private int m_propertyID = -1;
        private UnityEngine.Material m_Material;

        public JTweenMaterialColor() {
            m_tweenType = (int)JTweenMaterial.Color;
            m_tweenElement = JTweenElement.Material;
        }

        public Color BeginColor {
            get {
                return m_beginColor;
            }
            set {
                m_beginColor = value;
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

        public string Property {
            get {
                return m_property;
            }
            set {
                m_property = value;
            }
        }

        public int PropertyID {
            get {
                return m_propertyID;
            }
            set {
                m_propertyID = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            var renderer = m_target.GetComponent<Renderer>();
            if (null != renderer) m_Material = renderer.material;
            // end if
            if (null == m_Material) return;
            // end if
            m_beginColor = m_Material.color;
        }

        protected override Tween DOPlay() {
            if (null == m_Material) return null;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                return m_Material.DOColor(m_toColor, m_property, m_duration);
            } else if (m_propertyID != -1) {
                return m_Material.DOColor(m_toColor, m_propertyID, m_duration);
            } // end if
            return m_Material.DOColor(m_toColor, m_duration);
        }

        public override void Restore() {
            if (null == m_Material) return;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                m_Material.SetColor(m_property, m_beginColor);
            } else if (m_propertyID != -1) {
                m_Material.SetColor(m_propertyID, m_beginColor);
            } // end if
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginColor")) BeginColor = JTweenUtils.JsonToColor(json.GetNode("beginColor"));
            // end if
            if (json.Contains("color")) m_toColor = JTweenUtils.JsonToColor(json.GetNode("color")); 
            // end if
            if (json.Contains("property")) m_property = json.GetString("property");
            // end if
            if (json.Contains("propertyID")) m_propertyID = json.GetInt("propertyID");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginColor", JTweenUtils.ColorJson(m_beginColor));
            json.SetNode("color", JTweenUtils.ColorJson(m_toColor));
            if (!string.IsNullOrEmpty(m_property)) {
                json.SetString("property", m_property);
            } // end if
            if (-1 != m_propertyID) {
                json.SetInt("propertyID", m_propertyID);
            } // end if
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Material) {
                errorInfo = GetType().FullName + " GetComponent<Renderer> is null or material is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}

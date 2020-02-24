﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Transform {
    public class JTweenTransformLookAt : JTweenBase {
        private Vector3 m_beginRotate = Vector3.zero;
        private Vector3 m_towards = Vector3.zero;
        private AxisConstraint m_axisConstraint = AxisConstraint.None;
        private Vector3 m_up = Vector3.up;
        private UnityEngine.Transform m_Transform;

        public Vector3 Towards {
            get {
                return m_towards;
            }
            set {
                m_towards = value;
            }
        }

        public AxisConstraint AxisConstraint {
            get {
                return m_axisConstraint;
            }
            set {
                m_axisConstraint = value;
            }
        }
        public Vector3 Up {
            get {
                return m_up;
            }
            set {
                m_up = value;
            }
        }

        public override void Init() {
            if (null == m_Target) return;
            // end if
            m_Transform = m_Target.GetComponent<UnityEngine.Transform>();
            if (null == m_Transform) return;
            // end if
            m_beginRotate = m_Transform.rotation.eulerAngles;
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            return m_Transform.DOLookAt(m_towards, m_Duration, m_axisConstraint, m_up);
        }

        protected override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.rotation = Quaternion.Euler(m_beginRotate);
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("towards")) m_towards = Utility.Utils.JsonToVector3(json["towards"]);
            // end if
            if (json.Contains("axis")) m_axisConstraint = (AxisConstraint)(int)json["axis"];
            // end if
            if (json.Contains("up")) m_up = Utility.Utils.JsonToVector3(json["up"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["towards"] = Utility.Utils.Vector3Json(m_towards);
            json["axis"] = (int)m_axisConstraint;
            json["up"] = Utility.Utils.Vector3Json(m_up);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Transform) {
                errorInfo = GetType().FullName + " GetComponent<Transform> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
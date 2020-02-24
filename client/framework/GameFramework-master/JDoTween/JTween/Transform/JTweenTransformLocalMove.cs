﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Transform {
    public class JTweenTransformLocalMove : JTweenBase {
        private enum MoveType {
            Move = 0,
            MoveX = 1,
            MoveY = 2,
            MoveZ = 3,
        }
        private MoveType m_MoveType = MoveType.Move;
        private Vector3 m_beginPosition = Vector3.zero;
        private Vector3 m_toPosition = Vector3.zero;
        private float m_toMoveX = 0;
        private float m_toMoveY = 0;
        private float m_toMoveZ = 0;
        private UnityEngine.Transform m_Transform;

        public Vector3 ToPosition {
            get {
                return m_toPosition;
            }
            set {
                m_MoveType = MoveType.Move;
                m_toPosition = value;
            }
        }

        public float ToMoveX {
            get {
                return m_toMoveX;
            }
            set {
                m_MoveType = MoveType.MoveX;
                m_toMoveX = value;
            }
        }

        public float ToMoveY {
            get {
                return m_toMoveY;
            }
            set {
                m_MoveType = MoveType.MoveY;
                m_toMoveY = value;
            }
        }

        public float ToMoveZ {
            get {
                return m_toMoveZ;
            }
            set {
                m_MoveType = MoveType.MoveZ;
                m_toMoveZ = value;
            }
        }

        public override void Init() {
            if (null == m_Target) return;
            // end if
            m_Transform = m_Target.GetComponent<UnityEngine.Transform>();
            if (null == m_Transform) return;
            // end if
            m_beginPosition = m_Transform.localPosition;
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            switch (m_MoveType) {
                case MoveType.Move:
                    return m_Transform.DOLocalMove(m_toPosition, m_Duration, m_IsSnapping);
                case MoveType.MoveX:
                    return m_Transform.DOLocalMoveX(m_toMoveX, m_Duration, m_IsSnapping);
                case MoveType.MoveY:
                    return m_Transform.DOLocalMoveY(m_toMoveY, m_Duration, m_IsSnapping);
                case MoveType.MoveZ:
                    return m_Transform.DOLocalMoveZ(m_toMoveZ, m_Duration, m_IsSnapping);
                default: return null;
            } // end switch
        }

        protected override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.localPosition = m_beginPosition;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("move")) {
                m_MoveType = MoveType.Move;
                m_toPosition = Utility.Utils.JsonToVector3(json["move"]);
            } else if (json.Contains("moveX")) {
                m_MoveType = MoveType.MoveX;
                m_toMoveX = (float)json["moveX"];
            } else if (json.Contains("moveY")) {
                m_MoveType = MoveType.MoveY;
                m_toMoveY = (float)json["moveY"];
            } else if (json.Contains("moveZ")) {
                m_MoveType = MoveType.MoveZ;
                m_toMoveZ = (float)json["moveZ"];
            } else {
                Debug.LogError(GetType().FullName + " JsonTo MoveType is null");
            } // end if
        }

        protected override void ToJson(ref JsonData json) {
            switch (m_MoveType) {
                case MoveType.Move:
                    json["move"] = Utility.Utils.Vector3Json(m_toPosition);
                    break;
                case MoveType.MoveX:
                    json["moveX"] = m_toMoveX;
                    break;
                case MoveType.MoveY:
                    json["moveY"] = m_toMoveY;
                    break;
                case MoveType.MoveZ:
                    json["moveZ"] = m_toMoveZ;
                    break;
                default:
                    Debug.LogError(GetType().FullName + " ToJson MoveType is null");
                    break;
            } // end swtich
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
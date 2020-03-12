using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BehaviorDesigner.Runtime;
namespace Game
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        public string[] rNewIconNameArr = new string[] {
       "Level_1_cloth1",
       "Level_1_cloth2",
       "Level_1_snow1",
       "Level_1_tree1",
       "Level_1_tree2",
       "Level_1_tree3",
       "Level_2_grass1(1)",
       "Level_2_grass1(2)",
       "Level_2_grass2_1",
       "Level_2_grass2_2",
       "Level_2_tree1",
       "Level_2_tree2_1",
       "Level_2_tree2_2",
       "Level_2_tree3_1(1)",
       "Level_2_tree3_1",
       "Level_2_tree3_2(1)",
       "Level_2_tree3_2",
       "Level_2_tree4_1",
       "Level_2_tree4_2",
        "Level_2_tree6",
        "Level_3_bu1",
        "Level_3_bu2",
        "Level_3_cloth3",
        "Level_4_cloth1",
        "Level_4_light",
        "Level_4_tree1",
        "Level_4_tree2",
        "Level_4_tree3",
        "Level_6_grass1",
        "Level_6_grass2",
        "Level_6_tree3",
        "Level_6_tree4",
        "Level_6_tree5",
        "Level_3_bu1"
    };
        public string[] rOldIconNameArr = new string[] {
        "Level_1_cloth1-Level_1_cloth1_",
        "Level_1_cloth2-Level_1_cloth2_",
        "Level_1_snow1-Level_1_snow1_",
        "Level_1_tree1-Level_1_tree1_",
        "Level_1_tree2-Level_1_tree2_",
        "Level_1_tree3-Level_1_tree3_",
        "Level_2_grass1-Level_2_grass1_",
        "Level_2_grass1-Level_2_grass1_",
        "Level_2_grass2_1-Level_2_grass2_1_",
        "Level_2_grass2_2-Level_2_grass2_2_",
        "Level_2_tree1-Level_2_tree1_",
        "Level_2_tree2_1-Level_2_tree2_1_",
        "Level_2_tree2_2-Level_2_tree2_2_",
        "Level_2_tree3_1-Level_2_tree3_1_",
        "Level_2_tree3_1-Level_2_tree3_1_",
        "Level_2_tree3_2-Level_2_tree3_2_",
        "Level_2_tree3_2-Level_2_tree3_2_",
        "Level_2_tree4_1-Level_2_tree4_1_",
        "Level_2_tree4_2-Level_2_tree4_2_",
        "Level_2_tree3_1-Level_2_tree3_1_",
        "Level_3_cloth1-play_"      ,
        "Level_3_cloth2-play_"      ,
        "Level_3_cloth3-play_"      ,
        "Level_4_cloth1-animation_" ,
        "Level_4_light1-play_"      ,
        "Level_4_tree1-play_"       ,
        "Level_4_tree2-play_"       ,
        "Level_4_tree3-play_"       ,
        "Level_6_grass1-play_"      ,
        "Level_6_grass2-play_"      ,
        "Level_6_tree1-play_"       ,
        "Level_6_tree2-play_"       ,
        "Level_6_tree3-play_",
        "Level_3_bu1_"
    };
        private Dictionary<string, GameObject> rActorCacheDic = new Dictionary<string, GameObject>();

        public const string rBehaviorTreeDir = "BehaviorTree";

        Dictionary<string, ExternalBehavior> rBehaviorCachDic = new Dictionary<string, ExternalBehavior>();

        Dictionary<string, CurvesControl> rCurvesFunCachDic = new Dictionary<string, CurvesControl>();

        public List<string> rFrameString = new List<string>();
#if UNITY_STANDALONE_WIN
        public const string WeatherPath = "WeatherPC";
#else
        public const string WeatherPath = "Weather";
#endif
        public string WeatherResName = "";
        public void LoadRole(string rName, int nId, bool bIsCreateStep, Action<GameObject> func)
        {
            GameObject rGo = null;
            var rKey = rName + "|" + nId;
            if (rActorCacheDic.ContainsKey(rKey))
            {
                rGo = rActorCacheDic[rKey];
            }
            else
            {
                var rCharactrerPrefab = UnityResLoader.LoadAsset<GameObject>("Actor:" + rName);
                if (bIsCreateStep)
                {
                    rGo = CreateCollision(rCharactrerPrefab.transform, nId);
                }
                else
                {
                    rGo = GameObject.Instantiate(rCharactrerPrefab.gameObject);
                    rGo.name = rGo.name + nId;
                    rActorCacheDic.Add(rKey, rGo);
                }
            }

            if (func != null)
            {
                func.Invoke(rGo);
            }
        }
        /// <summary>
        /// 加入缓存池
        /// </summary>
        /// <param name="rName"></param>
        public void LoadRoleToRoleCache(string rName, int nId, bool bIsInteractItem = false)
        {
            var rKey = rName + "|" + nId;
            if (!rActorCacheDic.ContainsKey(rKey))
            {
                var rCharactrerPrefab = UnityResLoader.LoadAsset<GameObject>("Actor:" + rName);
                if (!bIsInteractItem)
                {
                    var rGo = CreateCollision(rCharactrerPrefab.transform, nId);
                    rActorCacheDic.Add(rKey, rGo);
                }
                else
                {
                    rActorCacheDic.Add(rKey, rCharactrerPrefab);
                }
                if (rName.Equals("MC"))
                {
                    UnityResLoader.UnloadAssetFalse("Actor:" + rName);
                }
            }
        }

        public void LoadEffect(string rFileName, string rName, Action<GameObject> func)
        {
            string rPath = string.Format("{0}:{1}", rFileName, rName);
            var rGo = UnityResLoader.LoadAsset<GameObject>(rPath);
            if (rGo != null)
            {
                func(rGo);
            }
            else
            {
                //Debug.LogErrorFormat("没有找到特效{0}", rPath);
            }
        }

        public CurvesControl LoadCurves(string rName)
        {
            var rPath = "Curves/" + rName;
            if (rCurvesFunCachDic.ContainsKey(rPath))
            {
                return rCurvesFunCachDic[rPath];
            }
            else
            {
                var rGo = UnityResLoader.LoadAsset<GameObject>(rPath);
                var rCc = rGo.GetComponent<CurvesControl>();
                if (rCc == null)
                {
                    //Debug.LogError(rPath + "没有CurvesControl");
                }
                rCurvesFunCachDic.Add(rPath, rCc);
                return rCc;
            }

        }

        /// <summary>
        /// 行为树
        /// </summary>
        public ExternalBehavior LoadExtBehavior(string rBehavior)
        {
            ExternalBehavior rExtBehavior = null;
            if (rBehaviorCachDic.ContainsKey(rBehavior))
            {
                rExtBehavior = rBehaviorCachDic[rBehavior];
            }
            else
            {
                rExtBehavior = Resources.Load<ExternalBehavior>(string.Format("{0}/{1}", rBehaviorTreeDir, rBehavior));
                if (null == rExtBehavior)
                {
                    //Debug.LogErrorFormat("没有找到名字为 {0} ,数据id为 {1} 的行为树", rBehavior, 0);
                    return null;
                }
                rBehaviorCachDic.Add(rBehavior, rExtBehavior);
            }
            return rExtBehavior;
        }

        public void LoadExtBehaviroToCache(string rBehavior)
        {
            if (!rBehaviorCachDic.ContainsKey(rBehavior))
            {
                var rExtBehavior = Resources.Load<ExternalBehavior>(string.Format("{0}/{1}", rBehaviorTreeDir, rBehavior));
                if (null == rExtBehavior)
                {
                    //Debug.LogErrorFormat("没有找到名字为 {0} ,数据id为 {1} 的行为树", rBehavior, 0);
                    return;
                }
                rBehaviorCachDic.Add(rBehavior, rExtBehavior);
            }
        }

        public GameObject CreateCollision(Transform transform, int nId)
        {
            GameObject rGo = GameObject.Instantiate(transform.gameObject);
            rGo.name = rGo.name + nId;
            var rBoxCol2D = transform.GetComponent<BoxCollider2D>();

            var rStepGo = new GameObject("StepTrigger");
            rStepGo.transform.parent = rGo.transform;
            rStepGo.transform.localPosition = Vector3.zero;
            rStepGo.layer = LayerMask.NameToLayer("StepCollition");
            var rStepCol2D = rStepGo.AddComponent<PolygonCollider2D>();
            float sizeY = rBoxCol2D.size.y / 4;
            var offset = new Vector2(rBoxCol2D.offset.x, rBoxCol2D.offset.y - sizeY * 3 / 2);
            var size = new Vector2(rBoxCol2D.size.x, sizeY);

            rStepCol2D.offset = offset;
            rStepCol2D.points = BuildPolygonCollider2DPoints(size, true, true);

            var rGroundGo = new GameObject("GroundTrigger");
            rGroundGo.transform.parent = rGo.transform;
            rGroundGo.transform.localPosition = Vector3.zero;
            rGroundGo.layer = LayerMask.NameToLayer("GroundCollition");

            var rGroundCol2D = rGroundGo.AddComponent<PolygonCollider2D>();
            rGroundCol2D.offset = rBoxCol2D.offset;
            rGroundCol2D.points = BuildPolygonCollider2DPoints(rBoxCol2D.size, true, true);
            rGo.transform.parent = ActorObjectMgr.Instance._trRoot.transform;
            ToolBox.SetActiveSafe(rGo, false);
            return rGo;
        }
        Vector2[] BuildPolygonCollider2DPoints(Vector2 size, bool isNormal, bool isForwordRight)
        {
            var points = new Vector2[5];
            var offsetY = (size.x / 2) * Mathf.Tan(GameConst.MaxGroundAngle * Mathf.Deg2Rad);

            points[3] = new Vector2(0, -size.y / 2);
            if (offsetY < size.y)
            {
                points[0] = new Vector2(isNormal ? -size.x / 2 : (isForwordRight ? -size.x / 2 : -size.x / 2), size.y / 2);
                points[1] = new Vector2(isNormal ? size.x / 2 : (isForwordRight ? size.x / 2 : size.x / 2), size.y / 2);
                points[2] = points[3] + new Vector2(isNormal ? size.x / 2 : (isForwordRight ? size.x / 2 : (points[1].x + points[3].x) / 2), offsetY);
                points[4] = points[3] + new Vector2(isNormal ? -size.x / 2 : (isForwordRight ? (points[0].x + points[3].x) / 2 : -size.x / 2), offsetY);
            }
            else
            {
                var offsetX = (size.y / Mathf.Tan(GameConst.MaxGroundAngle * Mathf.Deg2Rad)) / 2;
                offsetY = offsetX * Mathf.Tan(GameConst.MaxGroundAngle * Mathf.Deg2Rad);
                points[0] = new Vector2(isNormal ? -offsetX : (isForwordRight ? -offsetX : -offsetX), size.y);
                points[1] = new Vector2(isNormal ? offsetX : (isForwordRight ? offsetX : offsetX), size.y);

                points[2] = points[3] + new Vector2(isNormal ? offsetX : (isForwordRight ? offsetX : (points[1].x + points[3].x) / 2), offsetY);
                points[4] = points[3] + new Vector2(isNormal ? -offsetX : (isForwordRight ? (points[0].x + points[3].x) / 2 : -offsetX), offsetY);
            }
            return points;
        }

        public void ClearResCache()
        {
            var rClearList = new List<string>();
            var en = rActorCacheDic.GetEnumerator();
            while (en.MoveNext())
            {
                if (!en.Current.Key.Equals("MC|9999"))
                {
                    rClearList.Add(en.Current.Key);
                }
            }
            var rABClearList = new List<string>();
            for (int i = 0; i < rClearList.Count; i++)
            {
                GameObject.Destroy(rActorCacheDic[rClearList[i]]);
                rActorCacheDic.Remove(rClearList[i]);
                var rKey = rClearList[i].Split('|');
                if (!rABClearList.Contains(rKey[0]))
                {
                    rABClearList.Add(rKey[0]);
                }
            }

            var e = rABClearList.GetEnumerator();
            while (e.MoveNext())
            {
                UnityResLoader.UnloadAsset("Actor:" + e.Current);
            }
            for (var i = 0; i < rFrameString.Count; i++)
            {
                UnityResLoader.UnloadAssetForceToZero(rFrameString[i]);
            }
            rFrameString.Clear();
            if (!string.IsNullOrEmpty(WeatherResName))
            {
                UnityResLoader.UnloadAssetForceToZero(WeatherPath + ":" + WeatherResName);
                WeatherResName = "";
            }
            rActorCacheDic.Clear();
            EffectManager.Instance.ClearEffectCach();
            var enInterItem = rInterItmeDic.GetEnumerator();
            while (enInterItem.MoveNext())
            {
                UnityResLoader.UnloadAsset("Actor:" + enInterItem.Current.Key);
            }
            rInterItmeDic.Clear();
        }

        private Dictionary<string, GameObject> rInterItmeDic = new Dictionary<string, GameObject>();

        public void LoadInterItem(string rName, Action<GameObject> func)
        {
            GameObject rItem = null;

            if (rInterItmeDic.ContainsKey(rName))
            {
                rItem = rInterItmeDic[rName];
            }
            else
            {
                rItem = UnityResLoader.LoadAsset<GameObject>("Actor:" + rName);
                rInterItmeDic.Add(rName, rItem);
            }

            if (func != null)
            {
                func.Invoke(rItem);
            }
        }

        public string GetSpriteName(string rNewName)
        {
            for (var i = 0; i < rNewIconNameArr.Length; i++)
            {
                if (rNewName == rNewIconNameArr[i])
                {
                    return rOldIconNameArr[i];
                }
            }
            //Debug.LogError("没有" + rNewName + "对应的名字");
            return "";
        }
    }
}

//using CatFramework;
//using CatFramework.Magics;
//using CatFramework.Tools;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace Assets.Test
//{
//    public class SkillUiTest : MonoBehaviour, IPointerClickHandler
//    {
//        class Skill : ISkill
//        {
//            Timer Timer;
//            public void Fire()
//            {
//                if (Timer.Ready())
//                {
//                    ConsoleCat.Log("触发");
//                    Timer.AppendDelay(1f);
//                    ConsoleCat.Log("冷却");
//                }
//            }
//        }
//        [SerializeField] RawImage rawImage;
//        [SerializeField] Image image;
//        [SerializeField] CanvasRenderer canvasRenderer;
//        [SerializeField] float delay = 1f;
//        MaterialPropertyBlock materialPropertyBlock;
//        Skill skill;
//        Timer Timer;
//        // Use this for initialization
//        void Start()
//        {
//            materialPropertyBlock = new MaterialPropertyBlock();
//            skill = new Skill();
//        }

//        // Update is called once per frame
//        void Update()
//        {
//            image.fillAmount = Timer.InversePercent(delay);
//        }

//        public void OnPointerClick(PointerEventData eventData)
//        {
//            if (Timer.Ready())
//            {
//                Timer.AppendDelay(delay);
//                skill.Fire();
//            }
//            //materialPropertyBlock.SetFloat("_TimeStart", Time.time); 无法设置到画布渲染器,如果一定要设置只能实例化材质,但每个材质的Drawcall高达4,以及就算同样的材质,如果不同的贴图,每个image的drawcall也是4,如果是默认材质则drawcall是每个image的drawcall是1;
//            //image.sprite.
//        }
//    }
//}
using GameLogic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MusicFilter : SingleMono<MusicFilter>
    {
        private static readonly int Show = Animator.StringToHash("show");
        public Animator categoryUI;
        public Image categoryFilterIcon;
        private Category currentCategory = Category.All;
        
        public void ToggleCategoryUI()
        {
            categoryUI.SetBool(Show, !categoryUI.GetBool(Show));
        }

        [VisibleEnum(typeof(Category))]
        public void SelectCategory(int category)
        {
            currentCategory = (Category) category;
            categoryFilterIcon.color = (Category) category == Category.All ? new Color(1f, 1f, 1f, 0.38f) : new Color(0f, 1f, 0.39f);
            MusicViewManager.Instance.FilterCategory((Category) category);
        }
        
        public void Search(string value)
        {
            MusicViewManager.Instance.FilterWord(value + " " + currentCategory);
        }
    }
}

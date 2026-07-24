using System;
using UnityEngine;
using VisualScript.Icons;

namespace VisualScript.Runtime.Alias
{
    [Get]

    [Title("Alias")]
    [Category("Alias/Alias")]

    [Description("Access to a game object by its Alias component ID")]
    [Image(typeof(IconBookmarkSolid), ColorTheme.Type.Red)]

    [Keywords("Shortcut", "Reference", "ID", "Identifier", "Bookmark", "Link")]
    [Keywords("Player", "Camera")]

    [Serializable]
    public class TypeGameObjectAlias : TypeGameObject
    {
        [SerializeField] private GetString m_Alias = new GetString(Alias.DEFAULT_ID);

        public override GameObject Get(Args args)
        {
            IdString id = new IdString(this.m_Alias.Get(args));
            return AliasManager.Get(id);
        }

        public override string ToString()
        {
            return this.m_Alias.ToString();
        }

        protected override SubscriptionMode Subscription => SubscriptionMode.OnChange;
    }
}
using System;
using UnityEngine;
using VisualScript.Icons;

namespace VisualScript.Runtime.Alias
{
    [Get, Set]

    [Title("Alias ID")]
    [Category("Alias/Alias ID")]

    [Description("The ID of an Alias component")]
    [Image(typeof(IconBookmarkSolid), ColorTheme.Type.Red, typeof(BadgeID))]

    [Keywords("Shortcut", "Reference", "ID", "Identifier", "Bookmark", "Link")]
    [Keywords("Player", "Camera")]

    [Serializable]
    public class TypeStringGameObjectAliasID : TypeString
    {
        [SerializeField] private GetGameObject m_Alias;

        public override string GetPreview(GameObject source)
        {
            GameObject gameObject = m_Alias.GetPreview(source);
            if (gameObject == null) return string.Empty;

            Alias alias = gameObject.GetComponent<Alias>();
            return alias != null ? alias.Id.String : string.Empty;
        }

        public TypeStringGameObjectAliasID() : this(new GetGameObject())
        { }

        public TypeStringGameObjectAliasID(GetGameObject gameObject)
        {
            this.m_Alias = gameObject;
        }

        public override string Get(Args args)
        {
            Alias alias = this.m_Alias.Get<Alias>(args);
            return alias != null ? alias.Id.String : string.Empty;
        }

        public override void Set(string value, Args args)
        {
            Alias alias = this.m_Alias.Get<Alias>(args);
            if (alias != null) alias.Id = new IdString(value);
        }

        public override string ToString()
        {
            return $"{this.m_Alias} ID";
        }

        protected override SubscriptionMode Subscription => SubscriptionMode.OnChange;
    }
}

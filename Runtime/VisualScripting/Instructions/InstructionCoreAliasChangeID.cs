using System;
using UnityEngine;
using VisualScript.Icons;

namespace VisualScript.Runtime.Alias
{
    [Title("Change Alias ID")]
    [Image(typeof(IconBookmarkSolid), ColorTheme.Type.Red, typeof(BadgeID))]

    [Description("Changes the ID of an Alias component")]
    [Category("Alias/Change Alias ID")]

    [Parameter("Alias", "The Alias component to change its ID")]
    [Parameter("ID", "The new ID of the Alias component")]

    [Keywords("Shortcut", "Reference", "ID", "Identifier", "Bookmark", "Link")]
    [Keywords("Player", "Camera")]

    [Serializable]
    public class InstructionCoreAliasChangeID : Instruction
    {
        [SerializeField] private GetGameObject m_Alias = new GetGameObject();
        [SerializeField] private GetString m_ID = new GetString();

        protected override string GetTitle(GameObject source) => $"{this.m_Alias} ID = {this.m_ID}";

        protected override Awaitable Run(Args args)
        {
            Alias alias = this.m_Alias.Get<Alias>(args);

            if (alias != null)
            {
                alias.Id = new IdString(this.m_ID.Get(args));
            }

            return Done;
        }
    }
}
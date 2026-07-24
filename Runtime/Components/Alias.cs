using System;
using UnityEngine;

namespace VisualScript.Runtime.Alias
{
    [AddComponentMenu("Visual Script/Alias")]
    [Icon("Packages/works.catsoft.visualscript-alias/Editor/Gizmos/GizmoAlias.png")]

    [DefaultExecutionOrder(UpdateManager.BEFORE_EARLY)]
    [DisallowMultipleComponent]

    [Serializable]
    public class Alias : InitMonoBehaviour
    {
        public const string DEFAULT_ID = "Player";

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private TypeId m_Mode;
        [SerializeField] private IdString m_Id = new IdString(DEFAULT_ID);

        // PROPERTIES: ----------------------------------------------------------------------------

        public IdString Id
        {
            get => this.m_Id;
            set
            {
                this.RequireAwakening();

                if (this.m_Id == value) return;
                IdString previousId = this.m_Id;

                this.m_Id = value;
                this.EventChange?.Invoke(previousId, this.m_Id);
            }
        }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<IdString, IdString> EventChange;

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnAwake()
        {
            if (this.m_Mode == TypeId.RandomID)
            {
                this.m_Id = IdString.Unique;
            }

            AliasManager.Register(this);
        }

        private void OnDestroy()
        {
            AliasManager.Unregister(this);
        }
    }
}
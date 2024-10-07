using System;
using UnityEngine;

namespace OSK
{
    public abstract class Ability : ScriptableObject
    {
        public string abilityName;
        public float cooldownTime;
        public float duration;
        public bool isOnCooldown;
        public event Action OnAbilityCooldownStarted;
        public event Action OnAbilityCooldownEnded;

        public bool IsActive
        {
            get => isActivated;
        }

        protected bool isActivated;

        public virtual void Activate(GameObject user)
        {
            isActivated = true;
        }

        public virtual void Deactivate(GameObject user)
        {
            isActivated = false;
        }

        public abstract void UpdateAbility(GameObject user);

        public virtual void StartCooldown()
        {
            OnAbilityCooldownStarted?.Invoke();
        }

        public virtual void EndCooldown()
        {
            OnAbilityCooldownEnded?.Invoke();
        }
    }
}
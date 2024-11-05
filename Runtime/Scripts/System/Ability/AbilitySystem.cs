using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    public class AbilitySystem : MonoBehaviour, IService
    {
        [ReadOnly, SerializeField] 
        private List<Ability> _abilities = new List<Ability>();

        private void Update()
        {
            foreach (var ability in _abilities)
            {
                if (ability.IsActive)
                {
                    ability.UpdateAbility(gameObject);
                }
            }
        }

        public void Activate(int index)
        {
            if (index < _abilities.Count && !_abilities[index].isOnCooldown)
            {
                _abilities[index].Activate(gameObject);
            }
        }
 
        public void Activate<AbilityType>() where AbilityType : Ability
        {
            foreach (var ability in _abilities)
            {
                if (ability is AbilityType && !ability.isOnCooldown)
                {
                    ability.Activate(gameObject);
                    return;
                }
            }
        }
 
        public void Active<AbilityType>() where AbilityType : Ability
        {
            foreach (var ability in _abilities)
            {
                if (ability is AbilityType && !ability.isOnCooldown)
                {
                    ability.Activate(gameObject);
                    return;
                }
            }
        }

        public void Deactivate(int index)
        {
            if (index < _abilities.Count && _abilities[index].IsActive)
            {
                _abilities[index].Deactivate(gameObject);
            }
        }
 
        public void Deactivate(Ability ability)
        {
            if (ability.IsActive)
            {
                ability.Deactivate(gameObject);
            }
        }

        public void DeActivate<AbilityType>() where AbilityType : Ability
        {
            foreach (var ability in _abilities)
            {
                if (ability is AbilityType && ability.IsActive)
                {
                    ability.Deactivate(gameObject);
                    return;
                }
            }
        }
 
        public void StartCooldown(Ability ability)
        {
            StartCoroutine(CooldownRoutine(ability));
        }

        private IEnumerator CooldownRoutine(Ability ability)
        {
            ability.StartCooldown();
            yield return new WaitForSeconds(ability.cooldownTime);
            ability.EndCooldown();
        }

        public void Add(Ability newAbility)
        {
            if (_abilities.Contains(newAbility))
            {
                return;
            }

            _abilities.Add(newAbility);
        }

        public void Remove(Ability ability)
        {
            if (_abilities.Contains(ability))
            {
                _abilities.Remove(ability);
            }
        }
    }
}
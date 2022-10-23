using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

// Modified after https://medium.com/@kryzarel/character-stats-attributes-in-unity-pt-1-70f90ade9788#:~:text=Character%20Stats%2FAttributes%20in%20Unity%20%28pt.%201%29%20%E2%80%94%20Keeping,of%20all%20those%20modifiers%20in%20an%20organized%20fashion.

namespace Stat.CharacterStats
{
    // Allows the class to be edited from the Unity Inspector
    [Serializable]
    public class CharacterStats
    {
        public float baseValue;
        protected bool _isDirty = true;

        // Only allow the variable to be assigned a value when it is
        // declared or inside the class constructor

        // Add flat bonuses to the stats first
        protected readonly List<StatModifier> _rawStatModifiers;
        public readonly ReadOnlyCollection<StatModifier> RawStatModifiers;

        // Apply percentages after the flat bonuses have been added already
        protected readonly List<StatModifier> _equipStatModifiers;
        public readonly ReadOnlyCollection<StatModifier> EquipStatModifiers;

        private float _lastBaseValue = float.MinValue;

        protected float _value;

        // Retrieve calculated final value, make sure this is called
        // only when the stat values have changed
        public virtual float Value
        {
            get
            {
                if (_isDirty || _lastBaseValue != baseValue)
                {
                    _lastBaseValue = baseValue;
                    _value = CalculateFinalValue();
                    _isDirty = false;
                }
                return _value;
            }
        }

        // Create list to store individual stat modifiers applied to stats and prohibits
        // the mutation of the list directly, but will change if statModifiers changes
        public CharacterStats()
        {
            _rawStatModifiers = new List<StatModifier>();
            RawStatModifiers = _rawStatModifiers.AsReadOnly();

            _equipStatModifiers = new List<StatModifier>();
            EquipStatModifiers = _equipStatModifiers.AsReadOnly();
        }

        public CharacterStats(float BaseValue) : this()
        {
            baseValue = BaseValue;
        }

        public virtual void AddRawModifier(StatModifier mod)
        {
            _isDirty = true;
            _rawStatModifiers.Add(mod);

            /**
            // Sort the modifier list based on CompareModifierOrder that compares
            // items based on our criteria
            _rawStatModifiers.Sort(CompareModifierOrder);
            **/
        }

        public virtual void AddEquipModifier(StatModifier mod)
        {
            _isDirty = true;
            _equipStatModifiers.Add(mod);

            /** 
            // Sort the modifier list based on CompareModifierOrder that compares
            // items based on our criteria
            _finalStatModifiers.Sort(CompareModifierOrder);
            **/
        }

        /**protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0; // if (a.Order == b.Order)
        }**/

        public virtual bool RemoveRawModifier(StatModifier mod)
        {
            if (_rawStatModifiers.Remove(mod))
            {
                _isDirty = true;
                return true;
            }
            return false;
        }

        public virtual bool RemoveEquipModifier(StatModifier mod)
        {
            if (_equipStatModifiers.Remove(mod))
            {
                _isDirty = true;
                return true;
            }
            return false;
        }

        /**public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = _rawStatModifiers.Count - 1; i >= 0; i--)
            {
                if (_rawStatModifiers[i].Source == source)
                {
                    _isDirty = true;
                    didRemove = true;
                    _rawStatModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }**/

        /**
        // Calculates the final stat value depending on the type of modifier within the list of modifiers
        protected virtual float CalculateFinalValue()
        {
            float finalValue = baseValue;
            float sumPercentAdd = 0; // Holds "PercentAdd" modifiers

            for (int i = 0; i < _rawStatModifiers.Count; i++)
            {
                StatModifier mod = _rawStatModifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= _rawStatModifiers.Count || _rawStatModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }
            **/

        // Apply the skill tree bonuses to the players base stats first, then apply the 
        // equipment bonuses next
        protected virtual float CalculateFinalValue() {
            float finalValue = baseValue;
            float sumPercentAdd = 0; // Holds "PercentAdd" modifiers

            // First calculate the base skill tree bonuses and add them to the base stat
            for (int i = 0; i < _rawStatModifiers.Count; i++) {
                StatModifier mod = _rawStatModifiers[i];

                if (mod.Type == StatModType.Flat) {
                    finalValue += mod.Value;

                } else if (mod.Type == StatModType.PercentAdd) {
                    sumPercentAdd += mod.Value;
                }
            }

            // After adding all the flat bonuses, multiply by the total percentage bonus
            finalValue *= 1 + sumPercentAdd;
            sumPercentAdd = 0;


            // Then calculate the equipment bonuses and add them to the base stat
            for (int i = 0; i < _equipStatModifiers.Count; i++) {
                StatModifier mod = _equipStatModifiers[i];

                if (mod.Type == StatModType.Flat) {
                    finalValue += mod.Value;

                } else if (mod.Type == StatModType.PercentAdd) {
                    sumPercentAdd += mod.Value;
                }
            }

            // After adding all the flat bonuses, multiply by the total percentage bonus
            finalValue *= 1 + sumPercentAdd;
            sumPercentAdd = 0;

            // 12.0001f != 12f
            // Requires using System call
            return (float)Math.Round(finalValue, 4);
        }
    }
}

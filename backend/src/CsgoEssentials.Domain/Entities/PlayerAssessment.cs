using CsgoEssentials.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CsgoEssentials.Domain.Entities
{
    public class PlayerAssessment : Entity, IValidatableObject
    {
        #region Properties

        /// <summary>
        /// Player to be assessed
        /// </summary>
        public User Player { get; set; }

        /// <summary>
        /// List of player attributes that will contains the grade for each skill 
        /// </summary>
        public IList<PlayerAttribute> PlayerAttributes { get; set; }

        /// <summary>
        /// Map that this player has the best performance
        /// </summary>
        public Map BestMap { get; set; }

        /// <summary>
        /// Map that this player has the worst performance
        /// </summary>
        public Map WorstMap { get; set; }

        /// <summary>
        /// Role that this player has the best performance
        /// </summary>
        public User PlayerRole1 { get; set; }

        /// <summary>
        /// Role that this player has the worst performance
        /// </summary>
        public User PlayerRole2 { get; set; }

        /// <summary>
        /// General strength points to be described
        /// </summary>
        /// 
        public User PlayerRole3 { get; set; }

        /// <summary>
        /// General strength points to be described
        /// </summary>
        public string StrengthPoints { get; set; }

        /// <summary>
        /// General weakness points to be described
        /// </summary>
        public string WeaknessPoints { get; set; }

        #endregion

        #region Methods

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

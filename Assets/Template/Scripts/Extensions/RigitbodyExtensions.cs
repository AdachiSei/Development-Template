using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Extension
{
    /// <summary>
    /// Rigitbodyの拡張メソッド
    /// </summary>
    public static class RigitbodyExtensions
    {
        #region Constraints Methods

        public static Rigidbody SetConstraints(this Rigidbody rb, params RigidbodyConstraints[] constraints)
        {
            var rigidbodyConstraints = RigidbodyConstraints.None;

            foreach (var constraint in constraints)
                rigidbodyConstraints |= constraint;

            rb.constraints = rigidbodyConstraints;
            return rb;
        }

        public static Rigidbody AddConstraints(this Rigidbody rb, RigidbodyConstraints constraints)
        {
            var rigidbodyConstraints = rb.constraints;
            rigidbodyConstraints |= constraints;
            rb.constraints = rigidbodyConstraints;
            return rb;
        }

        #endregion
    }
}
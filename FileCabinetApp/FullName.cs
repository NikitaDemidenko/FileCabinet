using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>Full name.</summary>
    public class FullName
    {
        /// <summary>Initializes a new instance of the <see cref="FullName"/> class.</summary>
        public FullName()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FullName"/> class.</summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <exception cref="ArgumentNullException">Thrown when firstName
        /// or
        /// lastName
        /// is null.</exception>
        public FullName(string firstName, string lastName)
        {
            this.FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            this.LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        }

        /// <summary>Gets or sets the first name.</summary>
        /// <value>First name.</value>
        [XmlAttribute("first")]
        public string FirstName { get; set; }

        /// <summary>Gets or sets the last name.</summary>
        /// <value>Last name.</value>
        [XmlAttribute("last")]
        public string LastName { get; set; }
    }
}

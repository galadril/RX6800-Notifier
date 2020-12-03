using System.Collections.Generic;

namespace RX6800.Notifier.Library.Model
{
    /// <summary>
    /// Defines the <see cref="Subscriber" />.
    /// </summary>
    public class Subscriber
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscriber"/> class.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <param name="interests">The interests<see cref="List{Videocard}"/>.</param>
        public Subscriber(string id, string email, List<Videocard> interests)
        {
            this.Id = id;
            this.Email = email;
            this.Interests = interests;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets the Email.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets or sets the Interests.
        /// </summary>
        public List<Videocard> Interests { get; set; }

        #endregion
    }
}

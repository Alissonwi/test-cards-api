using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cards.Infra.Models;

namespace Cards.Infra.Services.Communication
{
    public class SaveCardResponse : BaseResponse
    {
        public Card Card { get; private set; }

        private SaveCardResponse(bool success, string message, Card card) : base(success, message)
        {
            Card = card;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="category">Saved category.</param>
        /// <returns>Response.</returns>
        public SaveCardResponse(Card card) : this(true, string.Empty, card)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveCardResponse(string message) : this(false, message, null)
        { }
    }
}

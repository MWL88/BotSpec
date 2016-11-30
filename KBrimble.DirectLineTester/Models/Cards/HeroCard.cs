﻿using System.Collections.Generic;

namespace KBrimble.DirectLineTester.Models.Cards
{
    /// <summary>
    /// A Hero card (card with a single, large image)
    /// </summary>
    public class HeroCard
    {
        public const string ContentType = "application/vnd.microsoft.card.hero";

        /// <summary>
        /// Initializes a new instance of the HeroCard class.
        /// </summary>
        public HeroCard()
        {
        }

        /// <summary>
        /// Initializes a new instance of the HeroCard class.
        /// </summary>
        public HeroCard(string title = default(string), string subtitle = default(string), string text = default(string),
            IList<CardImage> images = default(IList<CardImage>), IList<CardAction> buttons = default(IList<CardAction>),
            CardAction tap = default(CardAction))
        {
            Title = title;
            Subtitle = subtitle;
            Text = text;
            Images = images;
            Buttons = buttons;
            Tap = tap;
        }

        /// <summary>
        /// Title of the card
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Subtitle of the card
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Text for the card
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Array of images for the card
        /// </summary>
        public IList<CardImage> Images { get; set; }

        /// <summary>
        /// Set of actions applicable to the current card
        /// </summary>
        public IList<CardAction> Buttons { get; set; }

        /// <summary>
        /// This action will be activated when user taps on the card itself
        /// </summary>
        public CardAction Tap { get; set; }
    }
}
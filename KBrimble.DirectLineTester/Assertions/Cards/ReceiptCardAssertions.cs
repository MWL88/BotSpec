﻿using System;
using System.Collections.Generic;
using KBrimble.DirectLineTester.Assertions.Cards.CardComponents;
using KBrimble.DirectLineTester.Exceptions;
using KBrimble.DirectLineTester.Models.Cards;

namespace KBrimble.DirectLineTester.Assertions.Cards
{
    public class ReceiptCardAssertions : IReceiptCardAssertions
    {
        private readonly ReceiptCard _receiptCard;
        private readonly StringHelpers<ReceiptCardAssertionFailedException> _stringHelpers;

        public ReceiptCardAssertions(ReceiptCard receiptCard) : this()
        {
            _receiptCard = receiptCard;
        }

        private ReceiptCardAssertions()
        {
            _stringHelpers = new StringHelpers<ReceiptCardAssertionFailedException>();
        }

        public IReceiptCardAssertions HasTitleMatching(string regex)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));

            _stringHelpers.TestForMatch(_receiptCard.Title, regex, CreateEx(nameof(_receiptCard.Title), regex));

            return this;
        }

        public IReceiptCardAssertions HasTitleMatching(string regex, string groupMatchRegex, out IList<string> matchedGroups)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));
            if (groupMatchRegex == null)
                throw new ArgumentNullException(nameof(groupMatchRegex));

            matchedGroups = _stringHelpers.TestForMatchAndReturnGroups(_receiptCard.Title, regex, groupMatchRegex, CreateEx(nameof(_receiptCard.Title), regex));

            return this;
        }

        public IReceiptCardAssertions HasTotalMatching(string regex)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));

            _stringHelpers.TestForMatch(_receiptCard.Total, regex, CreateEx(nameof(_receiptCard.Total), regex));

            return this;
        }

        public IReceiptCardAssertions HasTotalMatching(string regex, string groupMatchRegex, out IList<string> matchedGroups)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));
            if (groupMatchRegex == null)
                throw new ArgumentNullException(nameof(groupMatchRegex));

            matchedGroups = _stringHelpers.TestForMatchAndReturnGroups(_receiptCard.Total, regex, groupMatchRegex, CreateEx(nameof(_receiptCard.Total), regex));

            return this;
        }

        public IReceiptCardAssertions HasTaxMatching(string regex)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));

            _stringHelpers.TestForMatch(_receiptCard.Tax, regex, CreateEx(nameof(_receiptCard.Tax), regex));

            return this;
        }

        public IReceiptCardAssertions HasTaxMatching(string regex, string groupMatchRegex, out IList<string> matchedGroups)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));
            if (groupMatchRegex == null)
                throw new ArgumentNullException(nameof(groupMatchRegex));

            matchedGroups = _stringHelpers.TestForMatchAndReturnGroups(_receiptCard.Tax, regex, groupMatchRegex, CreateEx(nameof(_receiptCard.Tax), regex));

            return this;
        }

        public IReceiptCardAssertions HasVatMatching(string regex)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));

            _stringHelpers.TestForMatch(_receiptCard.Vat, regex, CreateEx(nameof(_receiptCard.Vat), regex));

            return this;
        }

        public IReceiptCardAssertions HasVatMatching(string regex, string groupMatchRegex, out IList<string> matchedGroups)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));
            if (groupMatchRegex == null)
                throw new ArgumentNullException(nameof(groupMatchRegex));

            matchedGroups = _stringHelpers.TestForMatchAndReturnGroups(_receiptCard.Vat, regex, groupMatchRegex, CreateEx(nameof(_receiptCard.Vat), regex));

            return this;
        }

        public ICardActionAssertions WithButtonsThat()
        {
            return new CardActionSetAssertions(_receiptCard.Buttons);
        }

        public ICardActionAssertions WithTapActionThat()
        {
            return new CardActionAssertions(_receiptCard.Tap);
        }

        public IFactAssertions WithFactThat()
        {
            return new FactSetAssertions(_receiptCard);
        }

        public IReceiptItemAssertions WithReceiptItemThat()
        {
            return new ReceiptItemSetAssertions(_receiptCard);
        }

        public Func<ReceiptCardAssertionFailedException> CreateEx(string testedProperty, string regex)
        {
            var message = $"Expected card to have property {testedProperty} to match {regex} but regex test failed";
            return () => new ReceiptCardAssertionFailedException(message);
        }
    }
}
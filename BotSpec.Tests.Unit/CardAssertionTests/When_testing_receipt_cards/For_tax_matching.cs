﻿using System;
using System.Collections.Generic;
using System.Linq;
using BotSpec.Assertions.Cards;
using BotSpec.Exceptions;
using FluentAssertions;
using Microsoft.Bot.Connector.DirectLine;
using NUnit.Framework;

namespace BotSpec.Tests.Unit.CardAssertionTests.When_testing_receipt_cards
{
    [TestFixture]
    public class For_a_tax_matching
    {
        [TestCase("some text")]
        [TestCase("")]
        [TestCase("symbols ([*])?")]
        public void TaxMatching_should_pass_if_regex_exactly_matches_message_Tax(string cardTaxAndRegex)
        {
            var receiptCard = new ReceiptCard(tax: cardTaxAndRegex);

            var sut = new ReceiptCardAssertions(receiptCard);

            Action act = () => sut.TaxMatching(cardTaxAndRegex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "SOME TEXT")]
        [TestCase(@"SYMBOLS ([*])?", @"symbols ([*])?")]
        public void TaxMatching_should_pass_regardless_of_case(string cardTax, string regex)
        {
            var receiptCard = new ReceiptCard(tax: cardTax);

            var sut = new ReceiptCardAssertions(receiptCard);

            Action act = () => sut.TaxMatching(regex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "so.*xt")]
        [TestCase("some text", "[a-z ]*")]
        [TestCase("some text", "s(ome tex)t")]
        public void TaxMatching_should_pass_when_using_standard_regex_features(string cardTax, string regex)
        {
            var receiptCard = new ReceiptCard(tax: cardTax);

            var sut = new ReceiptCardAssertions(receiptCard);

            Action act = () => sut.TaxMatching(regex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "some text!")]
        [TestCase("some text", "^[j-z ]*$")]
        [TestCase("some text", "s{12}")]
        public void TaxMatching_should_throw_ReceiptCardAssertionFailedException_for_non_matching_regexes(string cardTax, string regex)
        {
            var receiptCard = new ReceiptCard(tax: cardTax);

            var sut = new ReceiptCardAssertions(receiptCard);

            Action act = () => sut.TaxMatching(regex);

            act.ShouldThrow<ReceiptCardAssertionFailedException>();
        }

        [Test]
        public void TaxMatching_should_not_output_matches_when_regex_does_not_match_text()
        {
            IList<string> matches = null;

            var receiptCard = new ReceiptCard(tax: "some text");

            var sut = new ReceiptCardAssertions(receiptCard);

            Action act = () => sut.TaxMatching("non matching regex", "(some text)", out matches);

            act.ShouldThrow<ReceiptCardAssertionFailedException>();
            matches.Should().BeNull();
        }

        [Test]
        public void TaxMatching_should_not_output_matches_when_groupMatchingRegex_does_not_match_text()
        {
            IList<string> matches;

            var receiptCard = new ReceiptCard(tax: "some text");

            var sut = new ReceiptCardAssertions(receiptCard);

            sut.TaxMatching("some text", "(non matching)", out matches);

            matches.Should().BeNull();
        }

        [Test]
        public void TaxMatching_should_output_matches_when_groupMatchingRegex_matches_text()
        {
            IList<string> matches;

            const string someText = "some text";
            var receiptCard = new ReceiptCard(tax: someText);

            var sut = new ReceiptCardAssertions(receiptCard);

            sut.TaxMatching(someText, $"({someText})", out matches);

            matches.First().Should().Be(someText);
        }

        [Test]
        public void TaxMatching_should_output_multiple_matches_when_groupMatchingRegex_matches_text_several_times()
        {
            IList<string> matches;

            const string someText = "some text";
            var receiptCard = new ReceiptCard(tax: someText);

            var sut = new ReceiptCardAssertions(receiptCard);

            const string match1 = "some";
            const string match2 = "text";
            sut.TaxMatching(someText, $"({match1}) ({match2})", out matches);

            matches.Should().Contain(match1, match2);
        }

        [Test]
        public void TaxMatching_should_throw_ReceiptCardAssertionFailedException_when_text_is_null()
        {
            var card = new ReceiptCard();

            var sut = new ReceiptCardAssertions(card);

            Action act = () => sut.TaxMatching("anything");

            act.ShouldThrow<ReceiptCardAssertionFailedException>();
        }

        [Test]
        public void TaxMatching_should_throw_ReceiptCardAssertionFailedException_when_trying_to_capture_groups_but_text_is_null()
        {
            IList<string> matches;
            var card = new ReceiptCard();

            var sut = new ReceiptCardAssertions(card);

            Action act = () => sut.TaxMatching("anything", "(.*)", out matches);

            act.ShouldThrow<ReceiptCardAssertionFailedException>();
        }

        [Test]
        public void TaxMatching_should_throw_ArgumentNullException_if_regex_is_null()
        {
            var card = new ReceiptCard();

            var sut = new ReceiptCardAssertions(card);

            Action act = () => sut.TaxMatching(null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TaxMatching_should_throw_ArgumentNullException_if_when_capturing_groups_regex_is_null()
        {
            IList<string> matches;

            var card = new ReceiptCard();

            var sut = new ReceiptCardAssertions(card);

            Action act = () => sut.TaxMatching(null, "(.*)", out matches);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TaxMatching_should_throw_ArgumentNullException_if_groupMatchRegex_is_null()
        {
            IList<string> matches;

            var card = new ReceiptCard();

            var sut = new ReceiptCardAssertions(card);

            Action act = () => sut.TaxMatching("(.*)", null, out matches);

            act.ShouldThrow<ArgumentNullException>();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using BotSpec.Assertions.Cards;
using BotSpec.Exceptions;
using BotSpec.Tests.Unit.TestData;
using FluentAssertions;
using Microsoft.Bot.Connector.DirectLine;
using NUnit.Framework;

namespace BotSpec.Tests.Unit.CardAssertionTests.When_testing_hero_card_sets
{
    [TestFixture]
    public class For_text_matching
    {
        [TestCase("some text")]
        [TestCase("")]
        [TestCase("symbols ([*])?")]
        public void TextMatching_should_pass_if_regex_exactly_matches_message_Text_of_one_card(string cardTextAndRegex)
        {
            var cards = HeroCardTestData.CreateHeroCardSetWithOneCardThatHasSetProperties(text: cardTextAndRegex);

            var sut = new HeroCardSetAssertions(cards);

            Action act = () => sut.TextMatching(cardTextAndRegex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "SOME TEXT")]
        [TestCase(@"SYMBOLS ([*])?", @"symbols ([*])?")]
        public void TextMatching_should_pass_if_regex_exactly_matches_Text_of_at_least_1_card_regardless_of_case(string cardText, string regex)
        {
            var cards = HeroCardTestData.CreateHeroCardSetWithOneCardThatHasSetProperties(text: cardText);

            var sut = new HeroCardSetAssertions(cards);

            Action act = () => sut.TextMatching(regex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "so.*xt")]
        [TestCase("some text", "[a-z ]*")]
        [TestCase("some text", "s(ome tex)t")]
        public void TextMatching_should_pass_when_using_standard_regex_features(string cardText, string regex)
        {
            var cards = HeroCardTestData.CreateHeroCardSetWithOneCardThatHasSetProperties(text: cardText);

            var sut = new HeroCardSetAssertions(cards);

            Action act = () => sut.TextMatching(regex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text!")]
        [TestCase("^[j-z ]*$")]
        [TestCase("s{12}")]
        public void TextMatching_should_throw_HeroCardAssertionFailedException_when_regex_matches_no_cards(string regex)
        {
            var cards = HeroCardTestData.CreateRandomHeroCards();

            var sut = new HeroCardSetAssertions(cards);

            Action act = () => sut.TextMatching(regex);

            act.ShouldThrow<HeroCardAssertionFailedException>();
        }

        [Test]
        public void TextMatching_should_throw_HeroCardAssertionFailedException_when_Text_of_all_cards_is_null()
        {
            var cards = Enumerable.Range(1, 5).Select(_ => new HeroCard()).ToList();

            var sut = new HeroCardSetAssertions(cards);

            Action act = () => sut.TextMatching(".*");

            act.ShouldThrow<HeroCardAssertionFailedException>();
        }

        [Test]
        public void TextMatching_should_throw_HeroCardAssertionFailedException_when_trying_to_capture_groups_but_Text_of_all_cards_is_null()
        {
            IList<string> matches;

            var cards = Enumerable.Range(1, 5).Select(_ => new HeroCard()).ToList();

            var sut = new HeroCardSetAssertions(cards);

            Action act = () => sut.TextMatching(".*", "(.*)", out matches);

            act.ShouldThrow<HeroCardAssertionFailedException>();
        }

        [Test]
        public void TextMatching_should_not_output_matches_when_regex_does_not_match_Text_of_any_cards()
        {
            IList<string> matches = null;

            var cards = HeroCardTestData.CreateRandomHeroCards();

            var sut = new HeroCardSetAssertions(cards);

            Action act = () => sut.TextMatching("non matching regex", "(some text)", out matches);

            act.ShouldThrow<HeroCardAssertionFailedException>();
            matches.Should().BeNull();
        }

        [Test]
        public void TextMatching_should_not_output_matches_when_groupMatchingRegex_does_not_match_Text_of_any_card()
        {
            IList<string> matches;

            var cards = HeroCardTestData.CreateRandomHeroCards();

            var sut = new HeroCardSetAssertions(cards);

            sut.TextMatching(".*", "(non matching)", out matches);

            matches.Should().BeNull();
        }

        [Test]
        public void TextMatching_should_output_matches_when_groupMatchingRegex_matches_Text_of_any_card()
        {
            IList<string> matches;

            const string someText = "some text";
            var cards = HeroCardTestData.CreateHeroCardSetWithOneCardThatHasSetProperties(text: someText);

            var sut = new HeroCardSetAssertions(cards);

            sut.TextMatching(someText, $"({someText})", out matches);

            matches.First().Should().Be(someText);
        }

        [Test]
        public void TextMatching_should_output_multiple_matches_when_groupMatchingRegex_matches_Text_several_times_for_a_single_card()
        {
            IList<string> matches;

            const string someText = "some text";
            var cards = HeroCardTestData.CreateHeroCardSetWithOneCardThatHasSetProperties(text: someText);

            var sut = new HeroCardSetAssertions(cards);

            const string match1 = "some";
            const string match2 = "text";
            sut.TextMatching(someText, $"({match1}) ({match2})", out matches);

            matches.Should().Contain(match1, match2);
        }

        [Test]
        public void TextMatching_should_output_multiple_matches_when_groupMatchingRegex_matches_Text_on_multiple_cards()
        {
            IList<string> matches;

            var cards = HeroCardTestData.CreateRandomHeroCards();
            cards.Add(new HeroCard(text: "some text"));
            cards.Add(new HeroCard(text: "same text"));

            var sut = new HeroCardSetAssertions(cards);

            sut.TextMatching(".*", @"(s[oa]me) (text)", out matches);

            matches.Should().Contain("some", "same", "text");
        }

        [Test]
        public void TextMatching_should_throw_ArgumentNullException_if_regex_is_null()
        {
            var cards = HeroCardTestData.CreateRandomHeroCards();

            var sut = new HeroCardSetAssertions(cards);

            Action act = () => sut.TextMatching(null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TextMatching_should_throw_ArgumentNullException_when_capturing_groups_if_regex_is_null()
        {
            IList<string> matches;

            var cards = HeroCardTestData.CreateRandomHeroCards();

            var sut = new HeroCardSetAssertions(cards);

            Action act = () => sut.TextMatching(null, "(.*)", out matches);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TextMatching_should_throw_ArgumentNullException_if_groupMatchRegex_is_null()
        {
            IList<string> matches;

            var cards = HeroCardTestData.CreateRandomHeroCards();

            var sut = new HeroCardSetAssertions(cards);

            Action act = () => sut.TextMatching("(.*)", null, out matches);

            act.ShouldThrow<ArgumentNullException>();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using KBrimble.DirectLineTester.Assertions.Cards;
using KBrimble.DirectLineTester.Exceptions;
using KBrimble.DirectLineTester.Models.Cards;
using NUnit.Framework;

namespace KBrimble.DirectLineTester.Tests.Unit.When_testing_card_images
{
    [TestFixture]
    public class For_a_url_matching
    {
        [Test]
        public void HasUrlMatching_should_throw_ArgumentNullException_when_regex_is_null()
        {
            var cardImage = new CardImage(url: "http://example.com");

            var sut = new CardImageAssertions(cardImage);

            Action act = () => sut.HasUrlMatching(null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void HasUrlMatching_should_throw_ArgumentNullException_when_groupMatchRegex_is_null()
        {
            IList<string> matches;

            var cardImage = new CardImage(url: "http://example.com");

            var sut = new CardImageAssertions(cardImage);

            Action act = () => sut.HasUrlMatching(".*", null, out matches);

            act.ShouldThrow<ArgumentNullException>();
        }

        [TestCase("some text")]
        [TestCase("")]
        [TestCase("symbols ([*])?")]
        public void HasUrlMatching_should_pass_if_regex_exactly_matches_image_url(string urlAndRegex)
        {
            var cardImage = new CardImage(url: urlAndRegex);

            var sut = new CardImageAssertions(cardImage);

            Action act = () => sut.HasUrlMatching(urlAndRegex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "SOME TEXT")]
        [TestCase("SYMBOLS ([*])?", "symbols ([*])?")]
        public void HasUrlMatching_should_pass_regardless_of_case(string cardUrl, string regex)
        {
            var cardImage = new CardImage(url: cardUrl);

            var sut = new CardImageAssertions(cardImage);

            Action act = () => sut.HasUrlMatching(regex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "so.*xt")]
        [TestCase("some text", "[a-z ]*")]
        [TestCase("some text", "s(ome tex)t")]
        public void HasUrlMatching_should_pass_when_using_standard_regex_features(string cardUrl, string regex)
        {
            var cardImage = new CardImage(url: cardUrl);

            var sut = new CardImageAssertions(cardImage);

            Action act = () => sut.HasUrlMatching(regex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "some text!")]
        [TestCase("some text", "^[j-z ]*$")]
        [TestCase("some text", "s{12}")]
        public void HasUrlMatching_should_throw_CardImageAssertionFailedException_for_non_matching_regexes(string cardUrl, string regex)
        {
            var cardImage = new CardImage(url: cardUrl);

            var sut = new CardImageAssertions(cardImage);

            Action act = () => sut.HasUrlMatching(regex);

            act.ShouldThrow<CardImageAssertionFailedException>();
        }

        [Test]
        public void HasUrlMatching_should_not_output_matches_when_regex_does_not_match_text()
        {
            IList<string> matches = null;

            var cardImage = new CardImage(url: "some text");

            var sut = new CardImageAssertions(cardImage);

            Action act = () => sut.HasUrlMatching("non matching regex", "(some text)", out matches);

            act.ShouldThrow<CardImageAssertionFailedException>();
            matches.Should().BeNull();
        }

        [Test]
        public void HasUrlMatching_should_not_output_matches_when_groupMatchingRegex_does_not_match_text()
        {
            IList<string> matches;

            var cardImage = new CardImage(url: "some text");

            var sut = new CardImageAssertions(cardImage);

            sut.HasUrlMatching("some text", "(non matching)", out matches);

            matches.Should().BeNull();
        }

        [Test]
        public void HasUrlMatching_should_output_matches_when_groupMatchingRegex_matches_text()
        {
            IList<string> matches;

            const string someText = "some text";
            var cardImage = new CardImage(url: someText);

            var sut = new CardImageAssertions(cardImage);

            sut.HasUrlMatching(someText, $"({someText})", out matches);

            matches.First().Should().Be(someText);
        }

        [Test]
        public void HasUrlMatching_should_output_multiple_matches_when_groupMatchingRegex_matches_text_several_times()
        {
            IList<string> matches;

            const string someText = "some text";
            var cardImage = new CardImage(url: someText);

            var sut = new CardImageAssertions(cardImage);

            var match1 = "some";
            var match2 = "text";
            sut.HasUrlMatching(someText, $"({match1}) ({match2})", out matches);

            matches.Should().Contain(match1, match2);
        }


        [Test]
        public void HasUrlMatching_should_throw_CardImageAssertionFailedException_when_text_is_null()
        {
            var cardImage = new CardImage();

            var sut = new CardImageAssertions(cardImage);

            Action act = () => sut.HasUrlMatching("anything");

            act.ShouldThrow<CardImageAssertionFailedException>();
        }

        [Test]
        public void HasUrlMatching_should_throw_CardImageAssertionFailedException_when_trying_to_capture_groups_but_text_is_null()
        {
            IList<string> matches;
            var cardImage = new CardImage();

            var sut = new CardImageAssertions(cardImage);

            Action act = () => sut.HasUrlMatching("anything", "(.*)", out matches);

            act.ShouldThrow<CardImageAssertionFailedException>();
        }
    }
}

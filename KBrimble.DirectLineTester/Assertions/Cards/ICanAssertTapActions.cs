﻿using KBrimble.DirectLineTester.Assertions.Cards.CardComponents;

namespace KBrimble.DirectLineTester.Assertions.Cards
{
    public interface ICanAssertTapActions
    {
        ICardActionAssertions WithTapActionThat();
    }
}
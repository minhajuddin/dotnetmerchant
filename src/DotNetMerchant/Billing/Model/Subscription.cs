#region License

// DotNetMerchant
// (http://dotnetmerchant.org)
// Copyright (c) 2010 Conatus Creative Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DotNetMerchant.Extensions;
using DotNetMerchant.Model;

namespace DotNetMerchant.Billing.Model
{
    /// <summary>
    /// A subscription is a <see cref="IPeriodicPayment" /> that can include a free or paid trial
    /// term prior to regular billings.
    /// </summary>
    [Serializable]
    public class Subscription : IEntity, IPeriodicPayment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="end">The ending date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        /// <param name="trialPeriod">The trial period.</param>
        public Subscription(string name, DateTime start, DateTime end, Period billingPeriod, Money billingAmount,
                            Period trialPeriod)
            : this(name, start, end, billingPeriod, billingAmount)
        {
            ApplyTrialPeriod(trialPeriod, 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// If the activated date is before the start date, a pro-rated amount is calculated.
        /// If the activated date is after the start date, they are swapped and a pro-rated amount is calculated.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="end">The ending date.</param>
        /// <param name="activated">The activated date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        /// <param name="trialPeriod">The trial period.</param>
        public Subscription(string name, DateTime start, DateTime end, DateTime activated, Period billingPeriod,
                            Money billingAmount, Period trialPeriod)
            : this(name, start, end, billingPeriod, billingAmount)
        {
            ApplyTrialPeriod(trialPeriod, 1);
            ApplyProratedAmount(start, billingAmount, activated);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        /// <param name="trialPeriod">The trial period.</param>
        public Subscription(string name, DateTime start, Period billingPeriod, Money billingAmount, Period trialPeriod)
            : this(name, start, billingPeriod, billingAmount)
        {
            ApplyTrialPeriod(trialPeriod, 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        /// <param name="trialPeriod">The trial period.</param>
        /// <param name="trialAmount">The trial amount.</param>
        public Subscription(string name, DateTime start, Period billingPeriod, Money billingAmount, Period trialPeriod,
                            Money trialAmount)
            : this(name, start, billingPeriod, billingAmount)
        {
            ApplyTrialPeriod(trialPeriod, trialAmount, 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// If the activated date is before the start date, a pro-rated amount is calculated.
        /// If the activated date is after the start date, they are swapped and a pro-rated amount is calculated.</summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="activated">The activated date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        /// <param name="trialPeriod">The trial period.</param>
        /// <param name="trialAmount">The trial amount.</param>
        public Subscription(string name, DateTime start, DateTime activated, Period billingPeriod, Money billingAmount,
                            Period trialPeriod, Money trialAmount)
            : this(name, start, billingPeriod, billingAmount)
        {
            ApplyTrialPeriod(trialPeriod, trialAmount, 1);
            ApplyProratedAmount(start, billingAmount, activated);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        /// <param name="trialPeriod">The trial period.</param>
        /// <param name="trialAmount">The trial amount.</param>
        /// <param name="trialOccurrences">The number of trial period occurrences.</param>
        public Subscription(string name, DateTime start, Period billingPeriod, Money billingAmount, Period trialPeriod,
                            Money trialAmount, int trialOccurrences)
            : this(name, start, billingPeriod, billingAmount)
        {
            ApplyTrialPeriod(trialPeriod, trialAmount, trialOccurrences);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// If the activated date is before the start date, a pro-rated amount is calculated.
        /// If the activated date is after the start date, they are swapped and a pro-rated amount is calculated.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="activated">The activated date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        /// <param name="trialPeriod">The trial period.</param>
        /// <param name="trialAmount">The trial amount.</param>
        /// <param name="trialOccurrences">The number of trial period occurrences.</param>
        public Subscription(string name, DateTime start, DateTime activated, Period billingPeriod, Money billingAmount,
                            Period trialPeriod, Money trialAmount, int trialOccurrences)
            : this(name, start, billingPeriod, billingAmount)
        {
            ApplyTrialPeriod(trialPeriod, trialAmount, trialOccurrences);
            ApplyProratedAmount(start, billingAmount, activated);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// If the activated date is before the start date, a pro-rated amount is calculated.
        /// If the activated date is after the start date, they are swapped and a pro-rated amount is calculated.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="end">The ending date.</param>
        /// <param name="activated">The activated date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        /// <param name="trialPeriod">The trial period.</param>
        /// <param name="trialAmount">The trial amount.</param>
        /// <param name="trialOccurrences">The number of trial period occurrences.</param>
        public Subscription(string name, DateTime start, DateTime end, DateTime activated, Period billingPeriod,
                            Money billingAmount, Period trialPeriod, Money trialAmount, int trialOccurrences)
            : this(name, start, end, billingPeriod, billingAmount)
        {
            ApplyTrialPeriod(trialPeriod, trialAmount, trialOccurrences);
            ApplyProratedAmount(start, billingAmount, activated);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// If the activated date is before the start date, a pro-rated amount is calculated.
        /// If the activated date is after the start date, they are swapped and a pro-rated amount is calculated.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        /// <param name="activated">The activated date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        public Subscription(string name, DateTime start, DateTime end, DateTime activated, Period billingPeriod,
                            Money billingAmount)
            : this(name, start, end, billingPeriod, billingAmount)
        {
            ApplyProratedAmount(start, billingAmount, activated);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="end">The ending date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        public Subscription(string name, DateTime start, DateTime end, Period billingPeriod, Money billingAmount)
        {
            Name = name;
            PeriodStartDate = start;
            PeriodEndDate = end;
            Period = billingPeriod;
            PaymentAmount = billingAmount;
            PaymentStartDate = start;
            TrialAmount = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="billingPeriod">The billing period.</param>
        /// <param name="billingAmount">The billing amount.</param>
        public Subscription(string name, DateTime start, Period billingPeriod, Money billingAmount)
        {
            Name = name;
            PeriodStartDate = start;
            Period = billingPeriod;
            PaymentAmount = billingAmount;
            PaymentStartDate = start;
            TrialAmount = null;
        }

        /// <summary>
        /// Gets or sets the name of this subscription.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the next payment date.
        /// </summary>
        /// <value>The next payment date.</value>
        public virtual DateTime? NextPaymentDate
        {
            get
            {
                var occurrences = PaymentDates;

                var result = occurrences.FirstOrDefault();

                return result == default(DateTime) ? (DateTime?) null : result;
            }
        }

        /// <summary>
        /// Gets all payment dates occurring in the current subscription.
        /// If the subscription has no end date, the next one hundred payment dates
        /// are returned.
        /// </summary>
        /// <value>The payment dates.</value>
        public virtual IEnumerable<DateTime> PaymentDates
        {
            get
            {
                var now = DateTime.UtcNow;

                if (now < PaymentStartDate)
                {
                    now = PaymentStartDate;
                }

                var then = PeriodEndDate.HasValue ? PeriodEndDate.Value : now.AddYears(100);

                var occurrences = Period.GetOccurrences(now, then);

                return occurrences;
            }
        }

        /// <summary>
        /// Gets the last payment date.
        /// </summary>
        /// <value>The last payment date.</value>
        public virtual DateTime? LastPaymentDate
        {
            get
            {
                if (!PeriodEndDate.HasValue)
                {
                    return null;
                }

                var now = DateTime.UtcNow;

                if (now > PeriodEndDate)
                {
                    return PeriodEndDate;
                }

                var then = PeriodEndDate.Value;

                var occurrences = Period.GetOccurrences(now, then);

                var result = occurrences.LastOrDefault();

                return result == default(DateTime) ? (DateTime?) null : result;
            }
        }

        /// <summary>
        /// Gets the previous payment date.
        /// </summary>
        /// <value>The previous payment date.</value>
        public virtual DateTime? PreviousPaymentDate
        {
            get
            {
                var now = DateTime.UtcNow;

                var then = PaymentStartDate;

                if (then >= now)
                {
                    return null;
                }

                var occurrences = Period.GetOccurrences(then, now);

                var result = occurrences.LastOrDefault();

                return result == default(DateTime) ? (DateTime?) null : result;
            }
        }


        /// <summary>
        /// Gets or sets the payment structure for this subscription that determines
        /// when trial payments are due, if the trial is not free.
        /// </summary>
        public Period? TrialPeriod { get; set; }

        /// <summary>
        /// Gets or sets the prorated amount due prior to the start of the
        /// subscription. If there is no prorated period, this is null.
        /// </summary>
        public Money? ProratedAmount { get; set; }

        /// <summary>
        /// Gets or sets the amount due on each trial payment period occurrence.
        /// </summary>
        public Money? TrialAmount { get; set; }

        /// <summary>
        /// Gets the total number of payments made in the life of this subscription
        /// If the trial period includes an amount, those payments are not included here.
        /// If the value cannot be determined, in the case where the subscription
        /// has no defined end date, it is null.
        /// </summary>
        public virtual int? BillingPayments
        {
            get
            {
                if (!PeriodEndDate.HasValue)
                {
                    return null;
                }

                var occurrences = Period.GetOccurrences(PaymentStartDate, PeriodEndDate.Value);

                return occurrences.Count();
            }
        }

        /// <summary>
        /// Gets the total number of trial payments in this subscription.
        /// This value is null if there is no trial period, or the
        /// trial period is free.
        /// </summary>
        public virtual int? TrialPayments
        {
            get
            {
                if (!TrialAmount.HasValue || TrialAmount == 0 || !TrialPeriod.HasValue)
                {
                    return null;
                }

                var occurrences = TrialPeriod.Value.GetOccurrences(PeriodStartDate, PaymentStartDate);

                var count = occurrences.Count();

                if (count == 0)
                {
                    return null;
                }

                return count;
            }
        }

        /// <summary>
        /// Gets the total amount of money paid during the life of this subscription.
        /// If there is a trial period with a non-zero payment amount, this is not included.
        /// If the value cannot be determined, it is infinite.
        /// </summary>
        public virtual Money TotalBillingAmount
        {
            get
            {
                if (!PeriodEndDate.HasValue)
                {
                    var infinity = PaymentAmount > 0
                                       ? double.PositiveInfinity
                                       : PaymentAmount < 0
                                             ? double.NegativeInfinity
                                             : 0;

                    return new Money(infinity);
                }

                var billingPayments = PaymentAmount*BillingPayments.Value;
                return billingPayments;
            }
        }

        /// <summary>
        /// Gets the total amount of money paid during the trial life of this subscription.
        /// If there is no trial period defined, or the trial is free, this is null.
        /// </summary>
        public virtual Money? TotalTrialAmount
        {
            get
            {
                if (!TrialAmount.HasValue || !TrialPayments.HasValue)
                {
                    return null;
                }

                var trialPayments = TrialAmount.Value*TrialPayments.Value;
                return trialPayments;
            }
        }

        /// <summary>
        /// This is the date subscription was activated.
        /// The payment start date is when regular billing occurs, whereas this is the date
        /// the subscription was activated. This is also the date the prorated subscription
        /// payment is due. If there is no prorated payment due, this is null.
        /// </summary>
        public DateTime? ProratedStartDate { get; set; }

        /// <summary>
        /// The unique identifier for this subscription assigned by an external provider.
        /// If you pass this instance to a payment processor, the ID assigned by the
        /// processor, if any, is assigned here.
        /// </summary>
        public Identity? ReferenceId { get; set; }

        #region IEntity Members

        ///<summary>
        /// The unique identifier for this subscription.
        /// You set this value if you intend to save this instance to a database.
        /// If you pass this instance to a payment processor, the ID assigned by the
        /// processor, if any, is assigned to <see cref="ReferenceId" />.
        ///</summary>
        public Identity Id { get; set; }

        #endregion

        #region IPeriodicPayment Members

        /// <summary>
        /// Gets or sets the payment structure for this subscription that determines
        /// when payments are due.
        /// </summary>
        public Period Period { get; set; }

        /// <summary>
        /// Gets or sets the amount due on each regular payment period occurrence.
        /// </summary>
        public Money PaymentAmount { get; set; }

        /// <summary>
        /// Gets the total number of payments made in the life of this subscription.
        /// If the trial period includes an amount, those payments are included here.
        /// If the value cannot be determined, in the case where the subscription
        /// has no defined end date, it is null.
        /// </summary>
        public virtual int? TotalPayments
        {
            get
            {
                var billingPayments = BillingPayments;

                if (billingPayments == null)
                {
                    return null;
                }

                var trialPayments = TrialPayments;

                var count = billingPayments + trialPayments.ValueOr(0);

                return count;
            }
        }

        /// <summary>
        /// Gets the total amount of money paid during the life of this subscription.
        /// If there is a trial period with a non-zero payment amount, this is included.
        /// If there is a pro-rated amount, this is included.
        /// If the value cannot be determined, it is infinite.
        /// </summary>
        public virtual Money TotalAmount
        {
            get
            {
                if (!PeriodEndDate.HasValue)
                {
                    var infinity = PaymentAmount > 0
                                       ? double.PositiveInfinity
                                       : PaymentAmount < 0
                                             ? double.NegativeInfinity
                                             : 0;

                    return new Money(infinity);
                }

                var billingPayments = PaymentAmount*BillingPayments.Value;

                var trialPayments = TrialAmount.HasValue && TrialPayments.HasValue
                                        ? TrialAmount.Value*TrialPayments.Value
                                        : 0;

                var proratedPayment = ProratedAmount.HasValue ? ProratedAmount.Value : 0;

                return billingPayments + trialPayments + proratedPayment;
            }
        }

        /// <summary>
        /// The start date of the this subscription.
        /// </summary>
        public DateTime PeriodStartDate { get; set; }

        /// <summary>
        /// The end date of this subscription, if known.
        /// </summary>
        public DateTime? PeriodEndDate { get; set; }

        /// <summary>
        /// The start date that regular billing period payments begin.
        /// </summary>
        public DateTime PaymentStartDate { get; set; }

        #endregion

        private void ApplyTrialPeriod(Period trialPeriod, int occurrences)
        {
            ApplyTrialPeriod(trialPeriod, null, occurrences);
        }

        private void ApplyTrialPeriod(Period trialPeriod, Money? trialAmount, int occurrences)
        {
            TrialAmount = trialAmount;
            TrialPeriod = trialPeriod;

            SetPaymentStartDateByTrialPeriod(occurrences);
        }

        private void ApplyProratedAmount(DateTime start, Money billingAmount, DateTime activated)
        {
            if (activated == start)
            {
                return;
            }

            if (activated > start)
            {
                var temp = activated;
                activated = start;
                start = temp;
            }

            var payments = PaymentDates.ToArray();
            if (payments.Length < 2)
            {
                return;
            }

            var secondPayment = payments[1];
            var offsetDays = DateSpan.GetDifference(DateInterval.Days, activated, start);
            var periodDays = DateSpan.GetDifference(DateInterval.Days, PaymentStartDate, secondPayment);
            if (periodDays == 0)
            {
                return;
            }

            var percentage = Convert.ToDouble(offsetDays)/Convert.ToDouble(periodDays);
            var prorated = billingAmount*percentage;

            ProratedAmount = prorated;
            ProratedStartDate = activated;
        }

        private void SetPaymentStartDateByTrialPeriod(int trialOccurrences)
        {
            if (!TrialPeriod.HasValue)
            {
                return;
            }

            var trialPeriod = TrialPeriod.Value;
            var calendar = CultureInfo.CurrentCulture.Calendar;

            if (PaymentStartDate.Kind == DateTimeKind.Utc)
            {
                // Calendar is always localized
                PaymentStartDate = PaymentStartDate.ToLocalTime();
            }

            var timeZone = TimeZone.CurrentTimeZone;
            var daylightPeriod = timeZone.GetDaylightChanges(PaymentStartDate.Year);

            for (var i = 0; i < trialOccurrences; i++)
            {
                switch (trialPeriod.Frequency)
                {
                    case PeriodFrequency.Days:
                        PaymentStartDate = calendar.AddDays(PaymentStartDate, trialPeriod.Quantifier);
                        break;
                    case PeriodFrequency.Weeks:
                        PaymentStartDate = calendar.AddWeeks(PaymentStartDate, trialPeriod.Quantifier);
                        break;
                    case PeriodFrequency.Months:
                        PaymentStartDate = calendar.AddMonths(PaymentStartDate, trialPeriod.Quantifier);
                        break;
                    case PeriodFrequency.Years:
                        PaymentStartDate = calendar.AddYears(PaymentStartDate, trialPeriod.Quantifier);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Future dates may mismatch on time of day due to DST
            if (PaymentStartDate > daylightPeriod.End)
            {
                PaymentStartDate = PaymentStartDate.Subtract(daylightPeriod.Delta);
            }

            PaymentStartDate = PaymentStartDate.ToUniversalTime();
        }
    }
}
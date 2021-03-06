﻿using Stryker.Core.Initialisation.ProjectComponent;
using Stryker.Core.Mutants;
using System.Collections.Generic;

namespace Stryker.Core.Reporters
{
    /// <summary>
    /// Broadcasts the report calls to all reporters in the list
    /// The order of the reporters is important, as the reporter invokes them in order
    /// </summary>
    public class BroadcastReporter : IReporter
    {
        private readonly object _mutex = new object();
        private IEnumerable<IReporter> _reporters { get; set; }

        public BroadcastReporter(IEnumerable<IReporter> reporters)
        {
            _reporters = reporters;
        }

        public void OnMutantsCreated(IReadOnlyInputComponent inputComponent)
        {
            foreach (var reporter in _reporters)
            {
                reporter.OnMutantsCreated(inputComponent);
            }
        }

        public void OnStartMutantTestRun(IEnumerable<Mutant> mutantsToBeTested)
        {
            foreach (var reporter in _reporters)
            {
                reporter.OnStartMutantTestRun(mutantsToBeTested);
            }
        }

        public void OnMutantTested(IReadOnlyMutant result)
        {
            lock (_mutex)
            {
                foreach (var reporter in _reporters)
                {
                    reporter.OnMutantTested(result);
                }
            }
        }

        public void OnAllMutantsTested(IReadOnlyInputComponent inputComponent)
        {
            foreach (var reporter in _reporters)
            {
                reporter.OnAllMutantsTested(inputComponent);
            }
        }
    }
}

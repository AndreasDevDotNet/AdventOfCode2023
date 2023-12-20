using AoCToolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    public class Solver : SolverBase
    {
        static Dictionary<string, Module> _moduleDictionary = new Dictionary<string, Module>();
        static Queue<Module> _processQueue = new Queue<Module>();
        static Dictionary<string, long> _rxIntersections = new();
        static string rxConnection;

        public Solver(string inputPath)
        {
            InputPath = inputPath;
            var inputData = GetInputText();

            parseModules(inputData);
        }

        public override string GetDayString()
        {
            return "* December 20th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Pulse Propagation";
        }

        public override void ShowVisualization()
        {
        }

        public override object Run(int part)
        {
            var value = part switch
            {
                1 => RunPart1(),
                2 => RunPart2()
            };

            return part switch
            {
                1 => $" Part #1\n  Number of High and Low pulses multiplied: {value}",
                2 => $" Part #2\n  Fewest button presses to deliver single low pulse to the module named rx: {value}",
            };
        }

        private object RunPart1()
        {
            var lowPulses = 0L;
            var highPulses = 0L;

            for (int i = 0; i < 1000; i++)
            {
                _moduleDictionary["button"].PulseMemory.Enqueue(("finger", false));
                _processQueue.Enqueue(_moduleDictionary["button"]);

                while(_processQueue.TryDequeue(out Module nextPulseTarget))
                {
                    (long pulsesTransmitted, bool pulseType) = _moduleDictionary[nextPulseTarget.Name].ProcessPulse();

                    if(pulseType)
                    {
                        highPulses += pulsesTransmitted;
                    }
                    else
                    {
                        lowPulses += pulsesTransmitted;
                    }
                }
            }

            return lowPulses * highPulses;
        }

        private object RunPart2()
        {
            foreach (var module in _moduleDictionary.Values)
            {
                module.Reset();
            }

            long cycleCounter = 1;

            while (!_rxIntersections.Values.All(x => x != 0))
            {
                _moduleDictionary["button"].PulseMemory.Enqueue(("finger", false));
                _processQueue.Enqueue(_moduleDictionary["button"]);

                while (_processQueue.TryDequeue(out Module nextPulseTarget))
                {
                    (_, bool pulseType) = _moduleDictionary[nextPulseTarget.Name].ProcessPulse();
                    if (_rxIntersections.ContainsKey(nextPulseTarget.Name) && _rxIntersections[nextPulseTarget.Name] == 0 && pulseType == true)
                    {
                        _rxIntersections[nextPulseTarget.Name] = cycleCounter;
                    }
                }

                cycleCounter++;
            }

            return _rxIntersections.Values.Aggregate(1L, (a, b) => a = Numerics.Lcm(a,b));
        }

        private void parseModules(string inputData)
        {
            foreach (var name in inputData.ExtractWords().Distinct())
            {
                var module = new Module() { Name = name };

                _moduleDictionary.Add(module.Name, module);
            }

            foreach (var line in inputData.SplitByNewline())
            {
                var ms = line.Split("->");
                var mn = ms[0].Trim();
                var mt = ' ';
                if (mn[0] == '&' || mn[0] == '%')
                {
                    mt = mn[0];
                    mn = mn.Substring(1);
                }
                var cm = ms[1].Trim();
                var mType = ModuleType.Uknown;
                var connectedM = cm.Split(',').ToList();
                connectedM.ForEach(x => x.Trim());

                var mnt = mn[0];
                mType = mt switch
                {
                    '%' => ModuleType.FlipFlop,
                    '&' => ModuleType.Conjunction,
                    _ => ModuleType.Broadcaster
                };

                _moduleDictionary[mn].ModuleType = mType;
                foreach (var m in connectedM)
                {
                    _moduleDictionary[mn].OutputTargets.Add(m.Trim());
                    _moduleDictionary[m.Trim()].LastReceivedPulses[mn] = false; 
                    if(m == "rx")
                    {
                        rxConnection = mn;
                    }
                }
                
            }

            var buttonModule = new Module()
            {
                Name = "button",
                ModuleType = ModuleType.Broadcaster,
                OutputTargets = new() { "broadcaster" }
            };

            _moduleDictionary["button"] = buttonModule;

            foreach (var mod in _moduleDictionary.Values.Where(x => x.OutputTargets.Contains(rxConnection)))
            {
                _rxIntersections[mod.Name] = 0;
            }
        }

        enum ModuleType
        {
            Uknown,
            Broadcaster,
            FlipFlop,
            Conjunction
        }

        class Module
        {
            public ModuleType ModuleType { get; set; }
            public string Name { get; set; }

            public bool FlipFlopState { get; set; }

            public List<string> OutputTargets { get; set; } = new();

            public Dictionary<string, bool> LastReceivedPulses { get; set; } = new();

            public Queue<(string source, bool pulseType)> PulseMemory { get; set; } = new();

            public void Reset()
            {
                FlipFlopState = false;
                foreach (var k in LastReceivedPulses.Keys)
                {
                    LastReceivedPulses[k] = false;
                }
            }

            public (long pulsesTransmitted, bool pulseType) ProcessPulse()
            {
                long pulsesTransmitted = 0;
                bool pulseType = false;
                if (PulseMemory.TryDequeue(out var pulse))
                {
                    if (ModuleType == ModuleType.FlipFlop)
                    {
                        if (!pulse.pulseType)
                        {
                            FlipFlopState = !FlipFlopState;
                            foreach (var moduleName in OutputTargets)
                            {
                                _moduleDictionary[moduleName].PulseMemory.Enqueue((Name, FlipFlopState));
                                _processQueue.Enqueue(_moduleDictionary[moduleName]);
                                pulsesTransmitted++;
                            }
                            pulseType = FlipFlopState;
                        }
                    }
                    else if (ModuleType == ModuleType.Conjunction)
                    {
                        LastReceivedPulses[pulse.source] = pulse.pulseType;

                        pulseType = LastReceivedPulses.Values.Any(x => !x);

                        foreach (var moduleName in OutputTargets)
                        {
                            _moduleDictionary[moduleName].PulseMemory.Enqueue((Name, pulseType));
                            _processQueue.Enqueue(_moduleDictionary[moduleName]);
                            pulsesTransmitted++;
                        }
                    }
                    else if (ModuleType == ModuleType.Broadcaster)
                    {
                        pulseType = pulse.pulseType;
                        foreach (var moduleName in OutputTargets)
                        {
                            _moduleDictionary[moduleName].PulseMemory.Enqueue((Name, pulseType));
                            _processQueue.Enqueue(_moduleDictionary[moduleName]);
                            pulsesTransmitted++;
                        }
                    }
                }

                return (pulsesTransmitted, pulseType);
            }
        }

    }
}

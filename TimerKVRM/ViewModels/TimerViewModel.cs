using System;
using System.Windows.Media;
using System.Windows.Threading;
using TimerKVRM.Infrastructure;
using TimerKVRM.Services;

namespace TimerKVRM.ViewModels;

public sealed class TimerViewModel : BaseViewModel
{
    private const int QuestionTotal = 60;
    private const int DupletSeg = 30;
    private const int BlitzSeg = 20;
    private const int BlitzCount = 3;
    private const int FinalTotal = 10;

    private readonly ISoundService _sound;
    private readonly DispatcherTimer _timer;

    private Mode _mode = Mode.None;
    private int _segmentIndex;
    private int _segmentRemaining;
    private int _segmentsCount;

    private Brush _timerBrush = Brushes.Black;

    private string _timerText = "60";
    private int _totalRemaining = QuestionTotal;
    private bool _waitingNextSegment;

    public TimerViewModel(ISoundService sound)
    {
        _sound = sound;

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += OnTick;

        StartQuestionCommand = new RelayCommand(StartQuestion, CanStartQuestion);
        StartDupletCommand = new RelayCommand(StartDupletOrContinue, CanStartDupletOrContinue);
        StartBlitzCommand = new RelayCommand(StartBlitzOrContinue, CanStartBlitzOrContinue);
        StopCommand = new RelayCommand(Stop, CanStop);

        ResetToIdle();
    }

    public string TimerText
    {
        get => _timerText;
        private set => Set(ref _timerText, value);
    }

    public Brush TimerBrush
    {
        get => _timerBrush;
        private set => Set(ref _timerBrush, value);
    }

    public RelayCommand StartQuestionCommand { get; }
    public RelayCommand StartDupletCommand { get; }
    public RelayCommand StartBlitzCommand { get; }
    public RelayCommand StopCommand { get; }

    private bool IsRunning => _timer.IsEnabled;

    private void RefreshCommands()
    {
        StartQuestionCommand.RaiseCanExecuteChanged();
        StartDupletCommand.RaiseCanExecuteChanged();
        StartBlitzCommand.RaiseCanExecuteChanged();
        StopCommand.RaiseCanExecuteChanged();
    }

    private bool CanStartQuestion()
    {
        return !IsRunning && !_waitingNextSegment && _mode is Mode.None;
    }

    private void StartQuestion()
    {
        _sound.PlayStart();
        _mode = Mode.Question;
        _totalRemaining = QuestionTotal;
        _segmentsCount = 1;
        _segmentIndex = 0;
        _segmentRemaining = QuestionTotal;
        TimerBrush = Brushes.Black;
        UpdateText();
        _timer.Start();
        RefreshCommands();
    }

    private bool CanStartDupletOrContinue()
    {
        return !IsRunning && (
            (_mode is Mode.None && !_waitingNextSegment) ||
            (_mode is Mode.Duplet && _waitingNextSegment)
        );
    }

    private void StartDupletOrContinue()
    {
        _sound.PlayStart();

        if (_mode == Mode.None)
        {
            _mode = Mode.Duplet;
            _totalRemaining = DupletSeg * 2;
            _segmentsCount = 2;
            _segmentIndex = 0;
        }
        else
        {
            _segmentIndex++;
        }

        _waitingNextSegment = false;
        _segmentRemaining = DupletSeg;
        TimerBrush = Brushes.Black;
        UpdateText();
        _timer.Start();
        RefreshCommands();
    }

    private bool CanStartBlitzOrContinue()
    {
        return !IsRunning && (
            (_mode is Mode.None && !_waitingNextSegment) ||
            (_mode is Mode.Blitz && _waitingNextSegment && _segmentIndex < BlitzCount)
        );
    }

    private void StartBlitzOrContinue()
    {
        _sound.PlayStart();

        if (_mode == Mode.None)
        {
            _mode = Mode.Blitz;
            _totalRemaining = BlitzSeg * BlitzCount;
            _segmentsCount = BlitzCount;
            _segmentIndex = 0;
        }
        else
        {
            _segmentIndex++;
        }

        _waitingNextSegment = false;
        _segmentRemaining = BlitzSeg;
        TimerBrush = Brushes.Black;
        UpdateText();
        _timer.Start();
        RefreshCommands();
    }

    private bool CanStop()
    {
        return IsRunning || _mode is Mode.Final || _waitingNextSegment;
    }

    public void Stop()
    {
        _sound.StopAll();
        _timer.Stop();
        ResetToIdle();
        RefreshCommands();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (_mode == Mode.None) return;

        if (_mode != Mode.Final)
        {
            _totalRemaining--;
            _segmentRemaining--;
            UpdateText();

            if (_totalRemaining == 11)
                _sound.PlayTenSeconds();

            if (_segmentRemaining != 0)
            {
                return;
            }

            _timer.Stop();

            if (_totalRemaining == 0)
            {
                BeginFinalCountdown();
            }
            else
            {
                _waitingNextSegment = true;
                _sound.PlayBreak();
                RefreshCommands();
            }
        }
        else
        {
            _totalRemaining--;
            UpdateText();

            if (_totalRemaining != 0)
            {
                return;
            }

            Stop();
            _sound.PlayFinalWarning();
        }
    }

    private void BeginFinalCountdown()
    {
        _mode = Mode.Final;
        _totalRemaining = FinalTotal;
        TimerBrush = Brushes.Red;
        UpdateText();
        _sound.PlayFinal();
        _timer.Start();
        RefreshCommands();
    }

    private void UpdateText()
    {
        TimerText = _totalRemaining.ToString();
    }

    private void ResetToIdle()
    {
        _mode = Mode.None;
        _waitingNextSegment = false;
        _totalRemaining = QuestionTotal;
        _segmentRemaining = 0;
        _segmentIndex = 0;
        _segmentsCount = 0;
        TimerBrush = Brushes.Black;
        TimerText = "60";
    }

    private enum Mode
    {
        None,
        Question,
        Duplet,
        Blitz,
        Final
    }
}
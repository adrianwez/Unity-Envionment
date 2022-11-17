using UnityEngine;
using System;
public class Realtime : MonoBehaviour
{
    [Header("Time Setup")]
    [SerializeField] private float _timeSpeed = 90;                // Speed in which time passes relative to real time.
    [SerializeField] private float _startHour = 10;                 // start at 10:00AM
    [SerializeField] private float _sunriseHour = 6;                // sunrise at 06:AM
    [SerializeField] private float _sunsetHour = 20;                // sunset at 08:00 PM
    private DateTime _currentTime;
    private DateTime _currentMinute;
    private DateTime _currentHour;
    private TimeSpan _sunriseTime;
    private TimeSpan _sunsetTime;

    [Header("Ambient")]
    [SerializeField] private Light _directional;
    [SerializeField] private Color _dayAmbient;
    [SerializeField] private Color _nightAmbient;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _maxLightIntensity;
    [SerializeField] private float _minLightIntensity;
    private float _lightRotation;
    private float _dot;

    // callbacks
    [Tooltip("A string formatted clock updated with the current time.")]
    public static string Clock { get; private set; }
    [Tooltip("Action invoked everytime a full 'minute' pass.")]
    public static Action OnMinutePass;
    [Tooltip("Action invoked everytime a full 'hour' pass.")]
    public static Action OnHourPass;
    // Initialization
    private void Start()
    {
        _currentTime = DateTime.Now.Date + TimeSpan.FromHours(_startHour);
        _currentHour = _currentTime;
        _currentMinute = _currentTime;

        _sunriseTime = TimeSpan.FromHours(_sunriseHour);
        _sunsetTime = TimeSpan.FromHours(_sunsetHour);

    }
    private void Update()
    {
        // Update time according to the engine.
        _currentTime = _currentTime.AddSeconds(Time.deltaTime * _timeSpeed);
        Clock = _currentTime.ToString("HH:mm");

        // if there's some component listening for a time change.
        if(_currentMinute.Minute != _currentTime.Minute)
        {
            OnMinutePass?.Invoke();
            if(_currentHour.Hour != _currentTime.Hour)
            {
                OnHourPass?.Invoke();
                _currentHour = _currentTime;
            }
            _currentMinute = _currentTime;
        }
        
        // Apply ambient visuals.
        VFX();
    }
    // check fo daytime transition.
    private TimeSpan TimeDifference(TimeSpan _from, TimeSpan _to)
    {
        TimeSpan _diff = _to - _from;

        if(_diff.TotalSeconds  < 0)
            _diff += TimeSpan.FromHours(24);
        return _diff;
    }

    // Changing visuals.
    private void VFX()
    {
        if(_currentTime.TimeOfDay > _sunriseTime && _currentTime.TimeOfDay < _sunsetTime)
        {
            // day time
            TimeSpan _sunriseToSunsetDuration = TimeDifference(_sunriseTime, _sunsetTime);
            TimeSpan _timeSinceSunrise = TimeDifference(_sunriseTime, _currentTime.TimeOfDay);
            
            // total percentage of the day that has passed
            double _perc = _timeSinceSunrise.TotalMinutes / _sunriseToSunsetDuration.TotalMinutes;

            _lightRotation = Mathf.Lerp(0, 180, (float)_perc);
        }else
        {
            // noon time
            TimeSpan _sunsetToSunriseDuration = TimeDifference(_sunsetTime, _sunriseTime);
            TimeSpan _timeSinceSunset = TimeDifference(_sunsetTime, _currentTime.TimeOfDay);

            // total percentage of the night that has passed
            double _perc = _timeSinceSunset.TotalMinutes / _sunsetToSunriseDuration.TotalMinutes;

            _lightRotation = Mathf.Lerp(180, 360, (float)_perc);
        }

        //
        _directional.transform.rotation = Quaternion.AngleAxis(_lightRotation, Vector3.right);

        _dot = Vector3.Dot(_directional.transform.forward, Vector3.down);
        if(_dot > 0)
            _directional.intensity = Mathf.Lerp(0, _maxLightIntensity, _curve.Evaluate(_dot));
        else _directional.intensity = _minLightIntensity;

        //
        RenderSettings.ambientLight = Color.Lerp(_nightAmbient, _dayAmbient, _curve.Evaluate(_dot));

        // // uncomment if using stary skybox
        // RenderSettings.skybox.SetColor("_SkyColor", Color.Lerp(_nightAmbient, _dayAmbient, _curve.Evaluate(_dot)));
    }
}

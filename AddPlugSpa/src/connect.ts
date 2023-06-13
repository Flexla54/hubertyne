async function onSubmit(e: Event) {
  e.preventDefault();

  const searchParams = new URLSearchParams(window.location.search);
  const provisionID = searchParams.get('id');
  const mqttIP = 'mqtt.hubertyne.me:80';

  let ssid = (document.getElementById('ssid') as HTMLInputElement).value;
  let pass = (document.getElementById('pw') as HTMLInputElement).value;

  await fetch('http://192.168.33.1/settings?allow_cross_origin=1').catch(); // Enable CORS
  await fetch(
    `http://192.168.33.1/settings?mqtt_enable=true
      &mqtt_id=${provisionID}
      &mqtt_user=${provisionID}
      &mqtt_pass=${provisionID}
      &mqtt_server=${mqttIP}`
  ); // Enable and configure MQTT
  await fetch(`http://192.168.33.1/settings?coiot_enable=0`); // Disable CoIot

  fetch(`http://192.168.33.1/reboot`); // Reboot device

  setTimeout(
    () =>
      fetch(
        `http://192.168.33.1/settings/sta?enabled=1&ssid=${ssid}&key=${pass}`
      ).then(() => {
        document.getElementById('working')!.hidden = true;
        document.getElementById('success')!.hidden = false;
      }),
    2000
  ); // Configure WiFi
}

function togglePassword() {
  var x = document.getElementById('pw') as HTMLInputElement;
  if (x.type === 'password') {
    x.type = 'text';
  } else {
    x.type = 'password';
  }
}

document.addEventListener(
  'DOMContentLoaded',
  function () {
    console.log('hello');
    let home = document.getElementById('homeLink');
    if (home) {
      home.setAttribute(
        'href',
        new URLSearchParams(window.location.search).get('success-redirect') ||
          'https://www.hubertyne.me/home'
      );
    }
  },
  false
);

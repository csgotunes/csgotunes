import React, { MouseEvent, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { getApiBaseUrl } from '../Utils';
import { UserProfileResponse } from '../Models/UserProfileResponse';
import FileSaver from 'file-saver';
import { clearSession, getSession } from '../Utils/AuthUtils';
import { useTranslation } from 'react-i18next';

export const DashboardPage: React.FunctionComponent<any> = () => {
  const history = useHistory();
  const [isDownloadingConfig, setDownloadingConfig] = useState(false);
  const { t } = useTranslation();

  useEffect(() => {
    const sessionId = getSession();

    if (sessionId === null) {
      clearSession();
      history.push('/login');
    }
  }, []);

  const downloadConfig = (e: MouseEvent<HTMLButtonElement>): void => {
    void (async function () {
      try {
        const sessionId = getSession();

        if (sessionId === null) {
          return;
        }

        const requestOptions = {
          method: 'GET',
          headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${sessionId}` }
        };

        setDownloadingConfig(true);
        const response = await fetch(getApiBaseUrl() + '/user-profile', requestOptions);

        if (response.status >= 400) {
          console.log('Oh, no!');
        }

        const userProfileResponse: UserProfileResponse = await response.json();
        const encodedSpotifyUserId = encodeURIComponent(userProfileResponse.spotifyUserID);
        const encodedCFGKey = encodeURIComponent(userProfileResponse.cfgKey);

        const userConfig = `
"CS:GO Tunes"
{
 "uri" "https://csgotunes.azurewebsites.net/api/game-state?spotifyUserID=${encodedSpotifyUserId}&cfgKey=${encodedCFGKey}"
 "timeout" "5.0"
 "buffer"  "0.1"
 "throttle" "0.5"
 "heartbeat" "60.0"
 "output"
 {
   "precision_time" "3"
   "precision_position" "1"
   "precision_vector" "3"
 }
 "data"
 {
   "provider"            "1"      // general info about client being listened to: game name, appid, client steamid, etc.
   "map"                 "1"      // map, gamemode, and current match phase ('warmup', 'intermission', 'gameover', 'live') and current score
   "round"               "1"      // round phase ('freezetime', 'over', 'live'), bomb state ('planted', 'exploded', 'defused'), and round winner (if any)
   "player_id"           "1"      // player name, clan tag, observer slot (ie key to press to observe this player) and team
   "player_state"        "1"      // player state for this current round such as health, armor, kills this round, etc.
   "player_weapons"      "1"      // output equipped weapons.
   "player_match_stats"  "1"      // player stats this match such as kill, assists, score, deaths and MVPs
 }
}
        `;

        const blob = new Blob([userConfig], {
          type: 'text/plain;charset=utf-8'
        });
        FileSaver.saveAs(blob, 'gamestate_integration_csgotunes.cfg');
      } finally {
        setDownloadingConfig(false);
      }
    })();
  };

  return (
    <div>
      <p>{t('dashboard_welcome')}</p>
      <button onClick={downloadConfig} disabled={isDownloadingConfig}>{t('download_cfg_button')}</button>
    </div>
  );
};

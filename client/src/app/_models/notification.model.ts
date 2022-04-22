export interface NotificationModel{
    id?: number;
    type: string;
    message: string;
    lobbyId?: number;
    inDiscordServer?: boolean;
}
export interface DiscordConnection{
    discordId: number;
    connected: boolean;
    username?: string;
    discriminator?: number;
    hidden: boolean;
}
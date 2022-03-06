export interface Lobby{
    id: number;
    maxUsers: number;
    title: string;
    lobbyDescription? : string;
    gameId: number;
    steamId: number;
    gameType: string;
    users: number[];
    adminUid: number;
    lobbyRequirement: {
        gender?: string;
    };
}
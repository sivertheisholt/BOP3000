export interface Lobby{
    gameId: number;
    title: string;
    id: number;
    lobbyDescription? : string;
    gameType: string;
    maxUsers: number;
    lobbyRequirement: {
        gender?: string;
    };
}